using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Relics
{
	public class Floganymai : SpiritProjectile
	{
		public override bool CanHaveImbueVFX => false;
		public override string Texture => AOUtils.BlankTexture;
		private int pulses;
		public override void SetDefaults()
		{
			base.SetDefaults();
			pulses = 0;
			Projectile.extraUpdates = 100;
			Projectile.timeLeft = 1000000;
			Projectile.height = 2;
			Projectile.width = 200;
		}

		public override bool? CanDamage() => false;

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				if (Projectile.owner == Main.myPlayer)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
				}
				Projectile.velocity = Vector2.UnitY;
			}

			if (Projectile.velocity.Y > .1f)
			{
				Imbue?.LingeringEffects(Projectile.Hitbox with { Width = Projectile.Hitbox.Height, X = Projectile.Hitbox.X + (Projectile.Hitbox.Width / 2) });
				SecondImbue?.LingeringEffects(Projectile.Hitbox with { Width = Projectile.Hitbox.Height, X = Projectile.Hitbox.X + (Projectile.Hitbox.Width / 2) });
			}
			else if (Projectile.numUpdates == 0)
			{
				Imbue?.LingeringEffects(Projectile.Hitbox);
				SecondImbue?.LingeringEffects(Projectile.Hitbox);
			}

			Projectile.ai[1] += 1f / Projectile.extraUpdates;

			if (Projectile.ai[1] >= 60)
			{
				Projectile.ai[1] = 0;
				var height = 250;

				Rectangle rect = new(Projectile.Hitbox.X, Projectile.Hitbox.Y - height, Projectile.width, height);
				if (Main.myPlayer == Projectile.owner)
				{
					rect = AOUtils.SimulateAOE(rect, Projectile.damage, Projectile.knockBack, Projectile, Projectile.DamageType, false, false);
				}
				for (int i = 0; i <= 10; i++)
				{
					Imbue?.LingeringEffects(rect);
					SecondImbue?.LingeringEffects(rect);
				}
				pulses++;
				if (pulses > 5)
				{
					Kill();
				}
			}

		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = false;
			width = height;
			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}
	}
}
