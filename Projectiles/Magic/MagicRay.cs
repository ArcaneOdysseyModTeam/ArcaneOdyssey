using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
			Main.projFrames[Type] = 4;
		}

		public Vector2 End
		{
			get
			{
				Vector2 proj = Projectile.Center;
				for (float i = 0; i < 85f * Projectile.Opacity; i++)
				{
					proj += Projectile.velocity;
					var tile = AOUtils.GetTile(proj.ToTileCoordinates().X, proj.ToTileCoordinates().Y);
					if (tile.IsTileReallySolidGround() || (!Imbue.CanBeWet && tile.LiquidAmount > 0))
					{
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

			if (!Main.dedServ)
			{
				if (Projectile.localAI[0]++ < (85f * Projectile.Opacity))
				{
					var proj = Projectile.Center;

					proj += Projectile.velocity * Projectile.localAI[0];

					var tile = AOUtils.GetTile(proj.ToTileCoordinates().X, proj.ToTileCoordinates().Y);
					if (!tile.IsTileReallySolidGround())
					{
						Imbue?.LingeringEffects(Projectile.Hitbox with { Location = (proj - (Projectile.Size / 2f)).ToPoint() });
						SecondImbue?.LingeringEffects(Projectile.Hitbox with { Location = (proj - (Projectile.Size / 2f)).ToPoint() });
					}
					else
					{
						Projectile.localAI[0] = 0;
					}
					
					
					if (tile.IsTileReallySolidGround() || (tile.LiquidAmount > 0))
					{
						Imbue?.KillEffects(Projectile.Hitbox with { Location = (proj - (Projectile.Size / 2f)).ToPoint() });
						SecondImbue?.KillEffects(Projectile.Hitbox with { Location = (proj - (Projectile.Size / 2f)).ToPoint() });
						Projectile.localAI[0] = 0;
					}
				}
				else
				{
					Projectile.localAI[0] = 0;
				}
			}
			
			if (AOPlayerOwner.myCircle is not null && !dying)
			{
				dying = AOPlayerOwner.myCircle.MarkedForDeath;
				Projectile.Opacity = AOPlayerOwner.myCircle.Projectile.Opacity;
				Projectile.velocity = AOPlayerOwner.myCircle.Projectile.rotation.ToRotationVector2() * Projectile.velocity.Length();
				Projectile.spriteDirection = (Projectile.velocity.X > 0).ToDirectionInt();
				Projectile.rotation = Projectile.velocity.ToRotation();
				Projectile.Center = AOPlayerOwner.myCircle.Projectile.Center - (Projectile.velocity * 1.75f);
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

		public override bool TouchingWater()
		{
			dying = true;
			Owner.channel = false;
			Projectile.ignoreWater = true;
			return true;
		}

		public override string Texture => AOUtils.GetTexture<MagicRay>().Replace(nameof(MagicRay), $"Rays/Normal/WindRayEnd");
		public Texture2D MidSprite => ArrayCollections.raySprites[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]?.Value ?? ArrayCollections.raySprites[ModContent.ItemType<WindMagic>()]?.Value ?? base.Sprite;
		public Texture2D EndSprite => ArrayCollections.rayEndSprites[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]?.Value ?? ArrayCollections.rayEndSprites[ModContent.ItemType<WindMagic>()]?.Value ?? base.Sprite;
		public Texture2D StartSprite => ArrayCollections.rayStartSprites[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]?.Value ?? ArrayCollections.rayStartSprites[ModContent.ItemType<WindMagic>()]?.Value ?? base.Sprite;

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float _ = 0f;
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, End, MathF.Sqrt((projHitbox.Width ^ 2) + (projHitbox.Height ^ 2)), ref _))
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
			var info = AOUtils.DrawChain(Projectile.Center, End, MidSprite, Projectile.scale, Main.projFrames[Type], Projectile.frame, Projectile.GetAlpha(), mode);
			var end = info.Ending + new Vector2(EndSprite.Width * Projectile.scale, 0).RotatedBy(Projectile.rotation);
			Main.EntitySpriteDraw(EndSprite, end - Main.screenPosition, EndSprite.Frame(1, Main.projFrames[Type], 0, info.FinalFrame), Projectile.GetAlpha(), Projectile.rotation, new Vector2(EndSprite.Width, EndSprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
			return false;
		}
	}
}
