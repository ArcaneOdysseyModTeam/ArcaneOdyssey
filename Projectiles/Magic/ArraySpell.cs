using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Magic
{
	public class ArraySpell : MagicSpell
	{
		// ai 2 is first frame bool
		public override string Texture => typeof(WindMagic).FullName.Replace('.', '/').Replace(nameof(WindMagic), ModContent.GetInstance<WindMagic>().AttackPrefix + "Blast");

		public override Texture2D Sprite => ArcaneOdysseyMod.Sets.blasts[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]?.Value ?? base.Sprite;

		public override float AOSize => .75f;

		public const int ShootDelay = 60;

		public const int ShootTime = 120;

		public override bool CanHaveImbueVFX => false;

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			if (Hovering)
				overWiresUI.Add(index);
		}

		public Rectangle Proj1 => new(Projectile.Center.X.Round(), Projectile.position.Y.Round() - (20 * Projectile.scale).Round(), (64 * Projectile.scale).Round(), (64 * Projectile.scale).Round());
		public bool Proj1Active = true;
		public Rectangle Proj2 => new(Proj1.X - (64 * Projectile.scale).Round(), Projectile.position.Y.Round() - (20 * Projectile.scale).Round(), (64 * Projectile.scale).Round(), (64 * Projectile.scale).Round());
		public bool Proj2Active = true;
		public Rectangle Proj3 => new(Proj1.X + (64 * Projectile.scale).Round(), Projectile.position.Y.Round() + (20 * Projectile.scale).Round(), (64 * Projectile.scale).Round(), (64 * Projectile.scale).Round());
		public bool Proj3Active = true;
		public Rectangle Proj4 => new(Proj2.X - (64 * Projectile.scale).Round(), Projectile.position.Y.Round() + (20 * Projectile.scale).Round(), (64 * Projectile.scale).Round(), (64 * Projectile.scale).Round());
		public bool Proj4Active = true;

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			base.ModifyHitNPC(target, ref modifiers);
			modifiers.SourceDamage /= 4;
		}


		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Proj1.Height + 40;
			Projectile.width = Proj1.Width + Proj2.Width + Proj3.Width + Proj4.Width;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 1;
			Projectile.timeLeft = ShootTime + ShootDelay;
			Projectile.penetrate = 4;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = Proj1.Width / 4;
			height = Proj1.Height / 4;
			fallThrough = true;
			return true;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Proj1Active);
			writer.Write(Proj2Active);
			writer.Write(Proj3Active);
			writer.Write(Proj4Active);
			writer.Write(Projectile.rotation);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Proj1Active = reader.ReadBoolean();
			Proj2Active = reader.ReadBoolean();
			Proj3Active = reader.ReadBoolean();
			Proj4Active = reader.ReadBoolean();
			Projectile.rotation = reader.ReadSingle();
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (Proj1Active && targetHitbox.Intersects(Proj1))
			{
				Imbue?.KillEffects(Proj1, Projectile);
				Proj1Active = false;
				return true;
			}
			if (Proj2Active && targetHitbox.Intersects(Proj2))
			{
				Imbue?.KillEffects(Proj2, Projectile);
				Proj2Active = false;
				return true;
			}
			if (Proj3Active && targetHitbox.Intersects(Proj3))
			{
				Imbue?.KillEffects(Proj3, Projectile);
				Proj3Active = false;
				return true;
			}
			if (Proj4Active && targetHitbox.Intersects(Proj4))
			{
				Imbue?.KillEffects(Proj4, Projectile);
				Proj4Active = false;
				return true;
			}
			return false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Projectile.owner == Main.myPlayer)
			{
				Projectile.netUpdate = true;
				Projectile.netSpam = 0;
			}
		}

		public Vector2 TrueCentre
		{
			get
			{
				Vector2? cent = null;

				if (Proj1Active)
				{
					cent ??= Proj1.Center();
					cent = Vector2.Lerp(cent.Value, Proj1.Center(), .5f);
				}

				if (Proj2Active)
				{
					cent ??= Proj2.Center();
					cent = Vector2.Lerp(cent.Value, Proj2.Center(), .5f);
				}

				if (Proj3Active)
				{
					cent ??= Proj3.Center();
					cent = Vector2.Lerp(cent.Value, Proj3.Center(), .5f);
				}

				if (Proj4Active)
				{
					cent ??= Proj4.Center();
					cent = Vector2.Lerp(cent.Value, Proj4.Center(), .5f);
				}

				return cent.GetValueOrDefault(Projectile.Center);
			}
		}

		public bool Hovering
		{
			get => Projectile.ai[0] == 0;
			set
			{
				if (value)
				{
					Projectile.ai[0] = 0;
				}
				else
				{
					Projectile.ai[0] = 1;
				}
			}
		}

		public int target = -1;

		public override void AI()
		{
			Projectile.hide = Hovering;
			if (Projectile.ai[2] == 0)
			{
				Projectile.ai[2] = 1;
				if (Projectile.owner == Main.myPlayer)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
				}
				Projectile.velocity = Vector2.Zero;
			}

			if (Projectile.frameCounter++ > 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()])
				{
					Projectile.frame = 0;
				}
			}

			if (Hovering)
			{
				if (Projectile.position.ToTileCoordinates() != Projectile.oldPosition.ToTileCoordinates() && Main.myPlayer == Projectile.owner)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
				}
				if (Imbue is not PhoenixMagic)
					Projectile.spriteDirection = Owner.direction;
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.Center = Projectile.Center.MoveTowards(Owner.RotatedRelativePoint(Owner.MountedCenter) - new Vector2(0, Player.defaultHeight * .75f * Projectile.scale), AOPlayerOwner.MaxPossibleSpeed * Imbue.ScrollSpeed);

					target = AOUtils.ClosestNPCAt(TrueCentre, ApplySpeed(12f) * ShootTime, false, true)?.whoAmI ?? -1;
					if (target != -1)
					{
						var targetnpc = Main.npc[target];
						Projectile.rotation = TrueCentre.DirectionTo(targetnpc.Center).ToRotation();
					}
					else if (Projectile.owner == Main.myPlayer)
					{
						Projectile.rotation = TrueCentre.AngleTo(Main.MouseWorld);
					}

					if (++Projectile.ai[1] > ShootDelay)
					{
						Hovering = false;
						Projectile.velocity = Projectile.rotation.ToRotationVector2() * ApplySpeed(12f);
						if (Main.myPlayer == Projectile.owner)
						{
							Projectile.netUpdate = true;
							Projectile.netSpam = 0;
						}
					}
				}
			}
			else
			{
				Imbue?.UpdateProjectile(Projectile);
				target = AOUtils.ClosestNPCAt(TrueCentre, ApplySpeed(12f) * Projectile.timeLeft, false, true)?.whoAmI ?? target;
				if (target != -1)
				{
					var targetnpc = Main.npc[target];
					Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(TrueCentre.AngleTo(targetnpc.Center + (targetnpc.velocity * 10)), ApplySpeed(MathHelper.TwoPi) / 100f).ToRotationVector2() * Projectile.velocity.Length();
				}
			}

			if (!Hovering || Imbue is SoundMagic)
			{
				if (Proj1Active)
				{
					Imbue?.LingeringEffects(Proj1, Projectile.velocity, Projectile);
					SecondImbue?.LingeringEffects(Proj1, Projectile.velocity, Projectile);
				}
				if (Proj2Active)
				{
					Imbue?.LingeringEffects(Proj2, Projectile.velocity, Projectile);
					SecondImbue?.LingeringEffects(Proj2, Projectile.velocity, Projectile);
				}
				if (Proj3Active)
				{
					Imbue?.LingeringEffects(Proj3, Projectile.velocity, Projectile);
					SecondImbue?.LingeringEffects(Proj3, Projectile.velocity, Projectile);
				}
				if (Proj4Active)
				{
					Imbue?.LingeringEffects(Proj4, Projectile.velocity, Projectile);
					SecondImbue?.LingeringEffects(Proj4, Projectile.velocity, Projectile);
				}
			}
		}

		public override void OnKill(int timeLeft)
		{
			if (Proj1Active)
			{
				Imbue?.KillEffects(Proj1, Projectile);
				SecondImbue?.KillEffects(Proj1, Projectile);
			}
			if (Proj2Active)
			{
				Imbue?.KillEffects(Proj2, Projectile);
				SecondImbue?.KillEffects(Proj2, Projectile);
			}
			if (Proj3Active)
			{
				Imbue?.KillEffects(Proj3, Projectile);
				SecondImbue?.KillEffects(Proj3, Projectile);
			}
			if (Proj4Active)
			{
				Imbue?.KillEffects(Proj4, Projectile);
				SecondImbue?.KillEffects(Proj4, Projectile);
			}
		}

		public override void PostDraw(Color lightColor)
		{
			if (ModContent.RequestIfExists<Texture2D>(GlowTexture, out var tex))
			{
				SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
				if (Proj1Active)
				{
					Lighting.AddLight(Proj1.Center(), Imbue.Colour.ToVector3() * Projectile.scale / 4f);
					Main.EntitySpriteDraw(Sprite, Proj1.Center() - Main.screenPosition, new(0, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()] * Projectile.frame, Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]) / 2f, Projectile.scale, mode);
					Main.EntitySpriteDraw(tex.Value, Proj1.Center() - Main.screenPosition, new(0, tex.Height() / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()] * Projectile.frame, tex.Width(), tex.Height() / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]), Projectile.GetAlpha(Color.White), Projectile.rotation, new Vector2(tex.Width(), tex.Height() / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]) / 2f, Projectile.scale, mode);
				}
				if (Proj2Active)
				{
					Lighting.AddLight(Proj2.Center(), Imbue.Colour.ToVector3() * Projectile.scale / 4f);
					Main.EntitySpriteDraw(Sprite, Proj2.Center() - Main.screenPosition, new(0, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()] * Projectile.frame, Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]) / 2f, Projectile.scale, mode);
					Main.EntitySpriteDraw(tex.Value, Proj2.Center() - Main.screenPosition, new(0, tex.Height() / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()] * Projectile.frame, tex.Width(), tex.Height() / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]), Projectile.GetAlpha(Color.White), Projectile.rotation, new Vector2(tex.Width(), tex.Height() / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]) / 2f, Projectile.scale, mode);
				}
				if (Proj3Active)
				{
					Lighting.AddLight(Proj3.Center(), Imbue.Colour.ToVector3() * Projectile.scale / 4f);
					Main.EntitySpriteDraw(Sprite, Proj3.Center() - Main.screenPosition, new(0, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()] * Projectile.frame, Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]) / 2f, Projectile.scale, mode);
					Main.EntitySpriteDraw(tex.Value, Proj3.Center() - Main.screenPosition, new(0, tex.Height() / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()] * Projectile.frame, tex.Width(), tex.Height() / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]), Projectile.GetAlpha(Color.White), Projectile.rotation, new Vector2(tex.Width(), tex.Height() / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]) / 2f, Projectile.scale, mode);
				}
				if (Proj4Active)
				{
					Lighting.AddLight(Proj4.Center(), Imbue.Colour.ToVector3() * Projectile.scale / 4f);
					Main.EntitySpriteDraw(Sprite, Proj4.Center() - Main.screenPosition, new(0, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()] * Projectile.frame, Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]) / 2f, Projectile.scale, mode);
					Main.EntitySpriteDraw(tex.Value, Proj4.Center() - Main.screenPosition, new(0, tex.Height() / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()] * Projectile.frame, tex.Width(), tex.Height() / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]), Projectile.GetAlpha(Color.White), Projectile.rotation, new Vector2(tex.Width(), tex.Height() / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]) / 2f, Projectile.scale, mode);
				}
			}
		}

		public override bool? CanDamage() => !Hovering;

		public override bool OnTileCollide(Vector2 oldVelocity) => !Hovering;

		public virtual void Rotate()
		{
			Projectile.spriteDirection = Projectile.direction;
			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : FlippedMode;
			if (Imbue is BlizzardMagic && !Hovering)
			{
				var texture = BlizzardMagic.trail;
				if (Proj1Active)
				{
					Main.EntitySpriteDraw(texture.Value, Proj1.Center() - (Projectile.velocity.SafeNormalize(Projectile.rotation.ToRotationVector2()) * (Proj1.Width / 2f)) - Main.screenPosition, new(0, texture.Height() / 7 * BlastSpell.TrailFrame, texture.Width(), texture.Height() / 7), Projectile.GetAlpha(lightColor), Projectile.velocity.SafeNormalize(Projectile.rotation.ToRotationVector2()).ToRotation(), new Vector2(texture.Width(), texture.Height() / 7) / 2f, Projectile.scale * .9f, SpriteEffects.None);
				}
				if (Proj2Active)
				{
					Main.EntitySpriteDraw(texture.Value, Proj2.Center() - (Projectile.velocity.SafeNormalize(Projectile.rotation.ToRotationVector2()) * (Proj2.Width / 2f)) - Main.screenPosition, new(0, texture.Height() / 7 * BlastSpell.TrailFrame, texture.Width(), texture.Height() / 7), Projectile.GetAlpha(lightColor), Projectile.velocity.SafeNormalize(Projectile.rotation.ToRotationVector2()).ToRotation(), new Vector2(texture.Width(), texture.Height() / 7) / 2f, Projectile.scale * .9f, SpriteEffects.None);
				}
				if (Proj3Active)
				{
					Main.EntitySpriteDraw(texture.Value, Proj3.Center() - (Projectile.velocity.SafeNormalize(Projectile.rotation.ToRotationVector2()) * (Proj3.Width / 2f)) - Main.screenPosition, new(0, texture.Height() / 7 * BlastSpell.TrailFrame, texture.Width(), texture.Height() / 7), Projectile.GetAlpha(lightColor), Projectile.velocity.SafeNormalize(Projectile.rotation.ToRotationVector2()).ToRotation(), new Vector2(texture.Width(), texture.Height() / 7) / 2f, Projectile.scale * .9f, SpriteEffects.None);
				}
				if (Proj4Active)
				{
					Main.EntitySpriteDraw(texture.Value, Proj4.Center() - (Projectile.velocity.SafeNormalize(Projectile.rotation.ToRotationVector2()) * (Proj4.Width / 2f)) - Main.screenPosition, new(0, texture.Height() / 7 * BlastSpell.TrailFrame, texture.Width(), texture.Height() / 7), Projectile.GetAlpha(lightColor), Projectile.velocity.SafeNormalize(Projectile.rotation.ToRotationVector2()).ToRotation(), new Vector2(texture.Width(), texture.Height() / 7) / 2f, Projectile.scale * .9f, SpriteEffects.None);
				}
				return false;
			}
			if (Proj1Active)
			{
				Main.EntitySpriteDraw(Sprite, Proj1.Center() - Main.screenPosition, new(0, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()] * Projectile.frame, Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]) / 2f, Projectile.scale, mode);
			}
			if (Proj2Active)
			{
				Main.EntitySpriteDraw(Sprite, Proj2.Center() - Main.screenPosition, new(0, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()] * Projectile.frame, Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]) / 2f, Projectile.scale, mode);
			}
			if (Proj3Active)
			{
				Main.EntitySpriteDraw(Sprite, Proj3.Center() - Main.screenPosition, new(0, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()] * Projectile.frame, Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]) / 2f, Projectile.scale, mode);
			}
			if (Proj4Active)
			{
				Main.EntitySpriteDraw(Sprite, Proj4.Center() - Main.screenPosition, new(0, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()] * Projectile.frame, Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]) / 2f, Projectile.scale, mode);
			}
			return false;
		}
	}
}
