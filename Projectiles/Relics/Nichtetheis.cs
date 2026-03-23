using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Projectiles.Base;
using ArcaneOdyssey.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Relics
{
	public class Nichtetheis : SpiritProjectile
	{
		public override string Texture
		{
			get
			{
				if (SecondImbue is AOMagic)
				{
					var asset = AOUtils.GetTexture<MagicRay>().Replace(nameof(MagicRay), $"Rays/{SecondImbue.ImbuableTier}/{SecondImbue.AttackPrefix}Ray");
					if (ModContent.HasAsset(asset))
					{
						return asset;
					}
				}
				return AOUtils.BlankTexture;
			}
		}

		public override Debuff? ProjectileDebuff => Debuff.Create<DrainedEffect>(60 * 5);
		public const int TravelTime = 75;
		public const int LingerTime = 100 * 60;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.projFrames[Type] = 5;
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
			if (Texture != AOUtils.BlankTexture)
			{
				SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : FlippedMode;
				var EndTexture = ModContent.Request<Texture2D>(Texture + "End");
				Main.EntitySpriteDraw(EndTexture.Value, Projectile.Center - Main.screenPosition, EndTexture.Frame(1, Main.projFrames[Type], 0, Projectile.frame), Projectile.GetAlpha(), Projectile.rotation + MathHelper.Pi, EndTexture.Size() with { Y = EndTexture.Height() / Main.projFrames[Type] } / 2f, Projectile.scale, mode);
				var ending = AOUtils.DrawChain(Projectile.Center, end.GetValueOrDefault(Owner.Center), Sprite, Projectile.scale, Main.projFrames[Type], Projectile.frame, Projectile.GetAlpha(), mode);
				ending += new Vector2(EndTexture.Width() * Projectile.scale, 0).RotatedBy(Projectile.rotation);
				Main.EntitySpriteDraw(EndTexture.Value, ending - Main.screenPosition, EndTexture.Frame(1, Main.projFrames[Type], 0, Projectile.frame), Projectile.GetAlpha(), Projectile.AngleTo(ending), EndTexture.Size() with { Y = EndTexture.Height() / Main.projFrames[Type] } / 2f, Projectile.scale, mode);
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

		public override bool? CanCutTiles() => SecondImbue is not null && !dying;

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.tileCollide = false;
			Projectile.timeLeft = LingerTime;
			return false;
		}

	}
}
