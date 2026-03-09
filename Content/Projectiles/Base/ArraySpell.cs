using ArcaneOdyssey.Content.Imbues.Magic.Lost;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class ArraySpell : MagicSpell
	{
		// ai 2 is first frame bool
		public override string Texture => GetType().FullName.Replace('.', '/').Replace("Array", "Blast");

		public override float AOSize => .75f;

		public const int ShootDelay = 60 * 3;

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
			Animate();
			Rotate();

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
					Projectile.Center = Projectile.Center.MoveTowards(Owner.RotatedRelativePoint(Owner.MountedCenter) - new Vector2(0, Player.defaultHeight * .75f * Projectile.scale), AOPlayerOwner.MaxPossibleSpeed * Imbue.AOScrollSpeed);

					target = Projectile.FindTargetWithLineOfSight(ApplySpeed(12f) * ShootTime);
					if (target != -1)
					{
						var targetnpc = Main.npc[target];
						if (ArcaneOdysseyConfig.Instance.PredictiveArray)
						{
							Projectile.rotation = Projectile.SafeDirectionTo(targetnpc.Center + (targetnpc.velocity * ApplySpeed(40f, true))).ToRotation();
						}
						else
						{
							Projectile.rotation = Projectile.SafeDirectionTo(targetnpc.Center).ToRotation();
						}
					}
					else if (Projectile.owner == Main.myPlayer)
					{
						Projectile.rotation = Projectile.Center.AngleTo(Main.MouseWorld);
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

		public override bool PreKill(int timeLeft)
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
			return base.PreKill(timeLeft);
		}

		public override void PostDraw(Color lightColor)
		{
			if (ModContent.RequestIfExists<Texture2D>(GlowTexture, out var tex))
			{
				SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
				if (Proj1Active)
				{
					Main.EntitySpriteDraw(Sprite, Proj1.Center() - Main.screenPosition, new(0, Sprite.Height / Main.projFrames[Type] * Projectile.frame, Sprite.Width, Sprite.Height / Main.projFrames[Type]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
					Main.EntitySpriteDraw(tex.Value, Proj1.Center() - Main.screenPosition, new(0, tex.Height() / Main.projFrames[Type] * Projectile.frame, tex.Width(), tex.Height() / Main.projFrames[Type]), Projectile.GetAlpha(Color.White), Projectile.rotation, new Vector2(tex.Width(), tex.Height() / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
				}
				if (Proj2Active)
				{
					Main.EntitySpriteDraw(Sprite, Proj2.Center() - Main.screenPosition, new(0, Sprite.Height / Main.projFrames[Type] * Projectile.frame, Sprite.Width, Sprite.Height / Main.projFrames[Type]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
					Main.EntitySpriteDraw(tex.Value, Proj2.Center() - Main.screenPosition, new(0, tex.Height() / Main.projFrames[Type] * Projectile.frame, tex.Width(), tex.Height() / Main.projFrames[Type]), Projectile.GetAlpha(Color.White), Projectile.rotation, new Vector2(tex.Width(), tex.Height() / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
				}
				if (Proj3Active)
				{
					Main.EntitySpriteDraw(Sprite, Proj3.Center() - Main.screenPosition, new(0, Sprite.Height / Main.projFrames[Type] * Projectile.frame, Sprite.Width, Sprite.Height / Main.projFrames[Type]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
					Main.EntitySpriteDraw(tex.Value, Proj3.Center() - Main.screenPosition, new(0, tex.Height() / Main.projFrames[Type] * Projectile.frame, tex.Width(), tex.Height() / Main.projFrames[Type]), Projectile.GetAlpha(Color.White), Projectile.rotation, new Vector2(tex.Width(), tex.Height() / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
				}
				if (Proj4Active)
				{
					Main.EntitySpriteDraw(Sprite, Proj4.Center() - Main.screenPosition, new(0, Sprite.Height / Main.projFrames[Type] * Projectile.frame, Sprite.Width, Sprite.Height / Main.projFrames[Type]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
					Main.EntitySpriteDraw(tex.Value, Proj4.Center() - Main.screenPosition, new(0, tex.Height() / Main.projFrames[Type] * Projectile.frame, tex.Width(), tex.Height() / Main.projFrames[Type]), Projectile.GetAlpha(Color.White), Projectile.rotation, new Vector2(tex.Width(), tex.Height() / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
				}
			}
		}

		public override bool? CanDamage() => !Hovering;

		public override bool OnTileCollide(Vector2 oldVelocity) => !Hovering;

		public virtual void Animate()
		{
			if (Projectile.frameCounter++ > 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Type])
				{
					Projectile.frame = 0;
				}
			}
		}

		public virtual void Rotate()
		{
			Projectile.spriteDirection = Projectile.direction;
			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
			if (Proj1Active)
			{
				Main.EntitySpriteDraw(Sprite, Proj1.Center() - Main.screenPosition, new(0, Sprite.Height / Main.projFrames[Type] * Projectile.frame, Sprite.Width, Sprite.Height / Main.projFrames[Type]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
			}
			if (Proj2Active)
			{
				Main.EntitySpriteDraw(Sprite, Proj2.Center() - Main.screenPosition, new(0, Sprite.Height / Main.projFrames[Type] * Projectile.frame, Sprite.Width, Sprite.Height / Main.projFrames[Type]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
			}
			if (Proj3Active)
			{
				Main.EntitySpriteDraw(Sprite, Proj3.Center() - Main.screenPosition, new(0, Sprite.Height / Main.projFrames[Type] * Projectile.frame, Sprite.Width, Sprite.Height / Main.projFrames[Type]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
			}
			if (Proj4Active)
			{
				Main.EntitySpriteDraw(Sprite, Proj4.Center() - Main.screenPosition, new(0, Sprite.Height / Main.projFrames[Type] * Projectile.frame, Sprite.Width, Sprite.Height / Main.projFrames[Type]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
			}
			return false;
		}
	}
}
