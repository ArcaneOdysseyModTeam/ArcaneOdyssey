using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Magic
{
	public class CannonSpell : MagicSpell
	{
		public int TileTimer = 0;

		public override string Texture => typeof(WindMagic).FullName.Replace('.', '/').Replace(nameof(WindMagic), ModContent.GetInstance<WindMagic>().AttackPrefix + "Blast");

		public override Texture2D Sprite => ArcaneOdysseyMod.Sets.blasts[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]?.Value ?? base.Sprite;

		public override float AOSize => 2f;

		public bool DoneCharging = false;
		public float charge = 1f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 64;
			Projectile.penetrate = -1;
			Projectile.localNPCHitCooldown = 30;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.timeLeft = 3 * 60;
			Projectile.hide = true;
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			if (!DoneCharging)
				overWiresUI.Add(index);
			else
				behindNPCsAndTiles.Add(index);
		}

		public override bool? CanDamage()
		{
			if (!DoneCharging)
			{
				return false;
			}
			return null;
		}

		public override void AI()
		{
			Projectile.tileCollide = !Projectile.wet;
			if (Projectile.ai[2] == 0f)
			{
				Projectile.ai[2] = 1f;
				if (Projectile.owner == Main.myPlayer)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0; ;
				}
			}
			if (Projectile.frameCounter++ > 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()])
				{
					Projectile.frame = 0;
				}
			}
			Imbue?.UpdateProjectile(Projectile);

			var dir = Main.myPlayer == Projectile.owner ? Owner.RotatedRelativePoint(Owner.MountedCenter).DirectionTo(Main.MouseWorld) : Projectile.DirectionFrom(Owner.RotatedRelativePoint(Owner.MountedCenter));

			if (Owner.channel && !DoneCharging)
			{
				if (Main.myPlayer == Projectile.owner && Projectile.position != Projectile.oldPosition)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
				}
				Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter) + (dir * 94f);
				charge += Circle.GlobalChargeSpeed;
				Projectile.timeLeft = 3 * 60;
				Projectile.rotation = dir.ToRotation();
				Projectile.spriteDirection = Owner.direction;
				if (charge >= Circle.GlobalMaxCharge)
				{
					Owner.channel = false;
					DoneCharging = true;
				}
			}
			else
			{
				DoneCharging = true;
				if (Projectile.velocity == Vector2.Zero)
				{
					Projectile.velocity = dir * ApplySpeed(5f);
					if (Main.myPlayer == Projectile.owner)
					{
						Projectile.netUpdate = true;
						Projectile.netSpam = 0;
					}
					if (ArcaneOdysseyClientConfig.Instance.AbilityText && Owner is not null && Owner.active && !Owner.DeadOrGhost && Main.myPlayer == Projectile.owner)
					{
						var name = (DisplayName + "!").Trim();
						name = (Imbue.PrettySpellPrefix + " " + name).Trim();
						if (SecondImbue is not null)
						{
							name = (SecondImbue.PrettyAttackPrefix + " " + name).Trim();
						}
						CombatText.NewText(Owner.Hitbox, Imbue.Colour, name.Trim(), true);
					}
				}
				if (TileTimer > 0)
					TileTimer--;
			}
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.SourceDamage *= charge;
		}

		public virtual void Rotate()
		{
			Projectile.spriteDirection = Projectile.direction;
			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width /= 4;
			height /= 4;
			fallThrough = true;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (DoneCharging)
			{
				if (TileTimer <= 0)
				{
					Imbue?.KillEffects(Projectile.Hitbox);
				}
				if (TileTimer < 60 && TileTimer > 0)
				{
					return true;
				}
				TileTimer = 65;
			}
			Projectile.position = Projectile.oldPosition;
			Projectile.velocity = oldVelocity;
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if (Imbue is BlizzardMagic)
			{
				//if (ModContent.RequestIfExists<Texture2D>(Texture + "_Overlay", out var texture))
				//{
				//	Main.EntitySpriteDraw(texture.Value, Projectile.Center - (Projectile.velocity.SafeNormalize(Projectile.rotation.ToRotationVector2()) * (Projectile.width / 2f)) - Main.screenPosition, new(0, texture.Width() * overlayFrame, texture.Width(), texture.Height() / OverlayFrames), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(texture.Width(), texture.Height() / OverlayFrames) / 2f, Projectile.scale * .9f, SpriteEffects.None);
				//}
			}
			SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : FlippedMode;
			Main.EntitySpriteDraw(Sprite, Projectile.Center - Main.screenPosition, new(0, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()] * Projectile.frame, Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]) / 2f, Projectile.scale, mode);
			return false;
		}
	}
}
