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
			Projectile.ArmorPenetration += 5;
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.projFrames[Type] = 4;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Opacity);
			writer.Write(dying);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			dying = reader.ReadBoolean();
			Opacity = reader.ReadSingle();
		}

		private float opac = 0f;

		public float Opacity
		{
			get
			{
				if (Projectile.owner == Main.myPlayer && !dying)
				{
					return AOPlayerOwner?.myCircle?.Projectile.Opacity ?? opac;
				}
				return opac;
			}
			set
			{
				opac = value;
			}
		}

		public Vector2 End
		{
			get
			{
				Vector2 proj = Projectile.Center;
				for (float i = 0; i < 85f * Opacity; i++)
				{
					proj += Projectile.velocity;
					var tile = AOUtils.GetTile(proj.ToTileCoordinates());
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
			Projectile.Opacity = Opacity;

			if (++Projectile.frameCounter > 6)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Type])
				{
					Projectile.frame = 0;
				}
			}
			
			if (AOPlayerOwner.myCircle is not null && !dying)
			{
				dying = AOPlayerOwner.myCircle.MarkedForDeath;
				Projectile.velocity = AOPlayerOwner.myCircle.Projectile.rotation.ToRotationVector2() * Projectile.velocity.Length();
				Projectile.spriteDirection = (Projectile.velocity.X > 0).ToDirectionInt();
				Projectile.Center = AOPlayerOwner.myCircle.Projectile.Center - (Projectile.velocity * 1.75f);
				if (!Main.dedServ)
				{
					var dist = Projectile.Distance(End);
					Imbue?.ConeEffects(Projectile.Center, dist, Projectile.rotation);
					SecondImbue?.ConeEffects(Projectile.Center, dist, Projectile.rotation);
				}
			}
			else
			{
				dying = true;

				Projectile.position -= Projectile.velocity;

				Opacity -= Circle.GlobalChargeSpeed * 2f;

				if (Projectile.alpha >= 255 && Main.myPlayer == Projectile.owner)
				{
					Kill();
				}
			}

			if (Projectile.position != Projectile.oldPosition)
			{
				NetUpdate();
			}
		}

		public override bool TouchingWater()
		{
			dying = true;
			Owner.channel = false;
			Projectile.ignoreWater = true;
			NetUpdate();
			return true;
		}

		public override string Texture => typeof(WindMagic).FullName.Replace('.', '/').Replace(nameof(WindMagic), ModContent.GetInstance<WindMagic>().AttackPrefix + "RayEnd");
		public Texture2D MidSprite => ArcaneOdysseyMod.Sets.Assets.raySprites[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]?.Value ?? ArcaneOdysseyMod.Sets.Assets.raySprites[ModContent.ItemType<WindMagic>()]?.Value ?? base.Sprite;
		public Texture2D EndSprite => ArcaneOdysseyMod.Sets.Assets.rayEndSprites[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]?.Value ?? ArcaneOdysseyMod.Sets.Assets.rayEndSprites[ModContent.ItemType<WindMagic>()]?.Value ?? base.Sprite;
		public Texture2D StartSprite => ArcaneOdysseyMod.Sets.Assets.rayStartSprites[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]?.Value ?? ArcaneOdysseyMod.Sets.Assets.rayStartSprites[ModContent.ItemType<WindMagic>()]?.Value ?? base.Sprite;

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float _ = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, End, projHitbox.Length(), ref _);
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if (AOPlayerOwner?.myCircle is not null)
				modifiers.SourceDamage *= Opacity;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : FlippedMode;
			var info = AOUtils.DrawChain(Projectile.Center, End, MidSprite, Projectile.scale, Main.projFrames[Type], Projectile.frame, Projectile.GetAlpha(), mode);
			var end = info.Ending + new Vector2(EndSprite.Width * Projectile.scale, 0).RotatedBy(info.Rotation);
			Main.EntitySpriteDraw(EndSprite, end - Main.screenPosition, EndSprite.Frame(1, Main.projFrames[Type], 0, info.FinalFrame), Projectile.GetAlpha(), info.Rotation, new Vector2(EndSprite.Width, EndSprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
			Projectile.rotation = info.Rotation;
			return false;
		}

		public override bool? CanCutTiles() => false;
	}
}
