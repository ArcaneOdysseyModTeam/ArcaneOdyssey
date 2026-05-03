using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Magic.Minions
{
	public class MinionMinionBeam : MagicSpell
	{
		public const int TravelTime = 400;
		public const int LingerTime = 100 * 60;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.projFrames[Type] = 4;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 10; // hitscan
			Projectile.stopsDealingDamageAfterPenetrateHits = true;
			Projectile.DamageType = DamageClass.MagicSummonHybrid;
			Projectile.extraUpdates = 100;
			Projectile.timeLeft = TravelTime + LingerTime;
			Projectile.frame = Main.rand.Next(Main.projFrames[Type]);
			Projectile.tileCollide = false;
		}


		public override string Texture => typeof(WindMagic).FullName.Replace('.', '/').Replace(nameof(WindMagic), ModContent.GetInstance<WindMagic>().AttackPrefix + "RayEnd");
		public Texture2D MidSprite => ArcaneOdysseyMod.Sets.Assets.raySprites[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]?.Value ?? ArcaneOdysseyMod.Sets.Assets.raySprites[ModContent.ItemType<WindMagic>()]?.Value ?? base.Sprite;
		public Texture2D EndSprite => ArcaneOdysseyMod.Sets.Assets.rayEndSprites[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]?.Value ?? ArcaneOdysseyMod.Sets.Assets.rayEndSprites[ModContent.ItemType<WindMagic>()]?.Value ?? base.Sprite;
		public Texture2D StartSprite => ArcaneOdysseyMod.Sets.Assets.rayStartSprites[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]?.Value ?? ArcaneOdysseyMod.Sets.Assets.rayStartSprites[ModContent.ItemType<WindMagic>()]?.Value ?? base.Sprite;


		public override float Size => .75f;


		private Vector2 origin = default;
		private Vector2? end = null;
		public override bool CanHaveImbueVFX => !dying;

		public bool dying = false;

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(dying);
			writer.WriteVector2(origin);
			writer.Write(end);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			dying = reader.ReadBoolean();
			origin = reader.ReadVector2();
			end = reader.ReadNullableVector2();
		}

		public override void AI()
		{
			if (origin == default)
			{
				origin = Projectile.Center;
				Projectile.spriteDirection = (Projectile.velocity.X > 0).ToDirectionInt();
			}

			if (Projectile.timeLeft <= LingerTime)
			{
				end ??= Projectile.Center;
				Projectile.Center = origin;
				Projectile.velocity = Vector2.Zero;
				dying = true;

				if (Projectile.numUpdates == 0)
					Projectile.Opacity -= Circle.GlobalChargeSpeed * 2f;

				if (Projectile.alpha >= 255 && Main.myPlayer == Projectile.owner)
				{
					Kill();
				}
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if (origin != default)
			{
				SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : FlippedMode;
				var info = AOUtils.DrawChain(Projectile.Center, end.GetValueOrDefault(origin), MidSprite, Projectile.scale, Main.projFrames[Type], Projectile.frame, Projectile.GetAlpha(), mode);
				var frame = StartSprite.Frame(1, Main.projFrames[Type], 0, Projectile.frame);
				Main.EntitySpriteDraw(StartSprite, Projectile.Center - Main.screenPosition, frame, Projectile.GetAlpha(), info.Rotation, frame.Size() / 2f, Projectile.scale, mode);
				var ending = info.Ending + new Vector2(EndSprite.Width * Projectile.scale, 0).RotatedBy(info.Rotation);
				Main.EntitySpriteDraw(EndSprite, ending - Main.screenPosition, EndSprite.Frame(1, Main.projFrames[Type], 0, info.FinalFrame), Projectile.GetAlpha(), info.Rotation, new Vector2(EndSprite.Width, EndSprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
			}
			return false;
		}

		public override bool? CanDamage()
		{
			if (!dying)
				return null;
			return false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.timeLeft = LingerTime;
		}
	}
}
