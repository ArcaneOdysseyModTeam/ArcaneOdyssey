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
		public Texture2D MidSprite => ArrayCollections.raySprites[SecondImbue?.Type ?? 0]?.Value ?? base.Sprite;
		public Texture2D EndSprite => ArrayCollections.rayEndSprites[SecondImbue?.Type ?? 0]?.Value ?? base.Sprite;
		public Texture2D StartSprite => ArrayCollections.rayStartSprites[SecondImbue?.Type ?? 0]?.Value ?? base.Sprite;

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
			Projectile.timeLeft = TravelTime + LingerTime;
			Projectile.frame = Main.rand.Next(Main.projFrames[Type]);
		}

		public override float AOSize => 1f;


		private Vector2? origin = null;
		private Vector2? end = null;
		public override bool CanHaveImbueVFX => false;

		public bool dying = false;

		public override void AI()
		{
			if (Projectile.timeLeft > LingerTime)
			{
				Projectile.rotation = Projectile.velocity.ToRotation();
				Projectile.spriteDirection = (Projectile.velocity.X > 0).ToDirectionInt();
				origin ??= Projectile.Center + (Projectile.velocity.SafeNormalize(Projectile.velocity) * 60f);
				Imbue?.LingeringEffects(Projectile.Hitbox, Projectile.velocity, Projectile);
				SecondImbue?.LingeringEffects(Projectile.Hitbox, Projectile.velocity, Projectile);
			}
			else
			{
				end ??= Projectile.Center;
				Projectile.Center = origin.GetValueOrDefault(Owner.Center);
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
			SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : FlippedMode;
			Main.EntitySpriteDraw(StartSprite, Projectile.Center - Main.screenPosition, StartSprite.Frame(1, Main.projFrames[Type], 0, Projectile.frame), Projectile.GetAlpha(), Projectile.AngleTo(end.GetValueOrDefault(Owner.Center)), new Vector2(StartSprite.Width, StartSprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
			var info = AOUtils.DrawChain(Projectile.Center, end.GetValueOrDefault(Owner.Center), MidSprite, Projectile.scale, Main.projFrames[Type], Projectile.frame, Projectile.GetAlpha(), mode);
			var ending = info.Ending + new Vector2(EndSprite.Width * Projectile.scale, 0).RotatedBy(Projectile.rotation);
			Main.EntitySpriteDraw(EndSprite, ending - Main.screenPosition, EndSprite.Frame(1, Main.projFrames[Type], 0, info.FinalFrame), Projectile.GetAlpha(), Projectile.rotation, new Vector2(EndSprite.Width, EndSprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
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

		public override bool? CanCutTiles() => SecondImbue is not null && !dying;

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.tileCollide = false;
			Projectile.timeLeft = LingerTime;
			return false;
		}

	}
}
