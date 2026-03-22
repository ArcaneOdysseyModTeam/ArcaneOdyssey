using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Magic
{
	public class MagicRay : MagicSpell
	{
		public override bool CanHaveImbueVFX => false;

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			overWiresUI.Add(index);
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 30; // thicker
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.hide = true;
			Projectile.alpha = 255 - 1;
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.projFrames[Type] = 5;
		}

		public Vector2 End
		{
			get
			{
				Vector2 proj = Projectile.Center;
				for (float i = 0; i < 85f * Projectile.Opacity; i++)
				{
					proj += Projectile.velocity;
					if (!Collision.CanHitLine(Projectile.Center, 0, 0, proj, 0, 0))
					{
						if (Main.rand.NextBool(25))
							Imbue?.KillEffects(Projectile.Hitbox with { Location = (proj - (Projectile.Size / 2f)).ToPoint() });
						break;
					}
				}
				return proj;
			}
		}

		public bool dying = false;

		public override void AI()
		{
			if (++Projectile.frameCounter > 6)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Type])
				{
					Projectile.frame = 0;
				}
			}
			if (Main.GameUpdateCount % 5 == 0)
			{
				for (Vector2 i = Vector2.Zero; i.Length() < Projectile.Center.Distance(End); i += Projectile.velocity)
				{
					Imbue?.LingeringEffects(Projectile.Hitbox with { Location = (Projectile.position + i).ToPoint() }, Projectile.velocity, Projectile);
					SecondImbue?.LingeringEffects(Projectile.Hitbox with { Location = (Projectile.position + i).ToPoint() }, Projectile.velocity, Projectile);
				}
			}
			if (AOPlayerOwner.myCircle is not null && !dying)
			{
				dying = AOPlayerOwner.myCircle.MarkedForDeath;
				Projectile.Opacity = AOPlayerOwner.myCircle.Projectile.Opacity;
				Projectile.velocity = AOPlayerOwner.myCircle.Projectile.rotation.ToRotationVector2() * Projectile.velocity.Length();
				Projectile.spriteDirection = (Projectile.velocity.X > 0).ToDirectionInt();
				Projectile.rotation = Projectile.velocity.ToRotation();
				Projectile.Center = AOPlayerOwner.myCircle.Projectile.Center - Projectile.velocity;
			}
			else
			{
				dying = true;

				Projectile.position -= Projectile.velocity;

				Projectile.Opacity -= Circle.GlobalChargeSpeed * 2f;

				if (Projectile.alpha >= 255 && Main.myPlayer == Projectile.owner)
				{
					Kill();
				}
			}
		}

		public string BackupTexture = AOUtils.GetTexture<MagicRay>().Replace(nameof(MagicRay), $"Rays/Normal/CrystalRay");

		public override string Texture
		{
			get
			{
				if (Imbue is not null)
				{
					var asset = AOUtils.GetTexture<MagicRay>().Replace(nameof(MagicRay), $"Rays/{Imbue.ImbuableTier}/{Imbue.AttackPrefix}Ray");
					if (ModContent.HasAsset(asset))
					{
						return asset;
					}
				}
				return BackupTexture;
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float _ = 0f;
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, End, projHitbox.Width, ref _))
			{
				return true;
			}

			return false;
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if (AOPlayerOwner?.myCircle is not null)
				modifiers.SourceDamage *= AOPlayerOwner.myCircle.Projectile.Opacity;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : FlippedMode;
			//lightColor = Imbue?.Colour ?? lightColor;
			var end = AOUtils.DrawChain(Projectile.Center - Projectile.velocity, End, Sprite, Projectile.scale, Main.projFrames[Type], Projectile.frame, Projectile.GetAlpha(lightColor), mode);
			var EndTexture = ModContent.Request<Texture2D>(Texture + "End");
			end += new Vector2(EndTexture.Width() * Projectile.scale, 0).RotatedBy(Projectile.rotation);
			Main.EntitySpriteDraw(EndTexture.Value, end - Main.screenPosition, EndTexture.Frame(1, Main.projFrames[Type], 0, Projectile.frame), Projectile.GetAlpha(lightColor), Projectile.AngleTo(end), EndTexture.Size() with { Y = EndTexture.Height() / Main.projFrames[Type] } / 2f, Projectile.scale, mode);
			return false;
		}
	}
}
