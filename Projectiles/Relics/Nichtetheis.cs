using ArcaneOdyssey;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace ArcaneOdyssey.Projectiles.Relics
{
	public class Nichtetheis : SpiritProjectile
	{
		public override string Texture => AOUtils.BlankTexture;
		public Texture2D MidSprite => ArcaneOdysseyMod.Sets.Assets.raySprites[SecondImbue?.Type ?? 0]?.Value ?? base.Sprite;
		public Texture2D EndSprite => ArcaneOdysseyMod.Sets.Assets.rayEndSprites[SecondImbue?.Type ?? 0]?.Value ?? base.Sprite;
		public Texture2D StartSprite => ArcaneOdysseyMod.Sets.Assets.rayStartSprites[SecondImbue?.Type ?? 0]?.Value ?? base.Sprite;

		public override Debuff? ProjectileDebuff => Debuff.Create<DrainedEffect>(60 * 5);
		public const int TravelTime = 75;
		public const int LingerTime = 100 * 60;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.projFrames[Type] = 4;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 40; // hitscan
			Projectile.extraUpdates = 100;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;
			Projectile.timeLeft = TravelTime + LingerTime;
			Projectile.frame = Main.rand.Next(Main.projFrames[Type]);
		}

		public override float Size => 1f;


		private Vector2 origin = default;
		private Vector2? end = null;
		public override bool CanHaveImbueVFX => !dying;

		public bool dying = false;

		public override void AI()
		{
			if (origin == default)
			{
				Projectile.spriteDirection = (Projectile.velocity.X > 0).ToDirectionInt();
				origin = Projectile.Center + (Projectile.velocity.SafeNormalize(Projectile.velocity) * 60f);
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

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = height = 0;
			fallThrough = true;
			return true;
		}

		public override bool? CanCutTiles() => !dying;

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.tileCollide = false;
			Projectile.timeLeft = LingerTime;
			return false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.timeLeft = LingerTime;
		}
	}
}
