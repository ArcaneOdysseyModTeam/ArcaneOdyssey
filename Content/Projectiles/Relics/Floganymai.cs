using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Relics
{
	public class Floganymai : SpiritProjectile
	{
		public override string Texture => Mod.Name + "/Backgrounds/Blank";
		private int pulses;
		public override void SetDefaults()
		{
			base.SetDefaults();
			pulses = 0;
			Projectile.extraUpdates = 100;
			Projectile.timeLeft = 1000000;
			Projectile.height = 2;
			Projectile.width = 300;
		}

		public override bool? CanDamage() => false;

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				Projectile.netUpdate = true;
				Projectile.velocity = Vector2.UnitY;
			}

			if (Projectile.velocity.Y > .1f)
			{
				Imbue?.LingeringEffects(Projectile.Hitbox with { Width = Projectile.Hitbox.Height, X = Projectile.Hitbox.X + (Projectile.Hitbox.Width / 2) } );
				SecondImbue?.LingeringEffects(Projectile.Hitbox with { Width = Projectile.Hitbox.Height, X = Projectile.Hitbox.X + (Projectile.Hitbox.Width / 2) });
			}
			else if (Main.GameUpdateCount % 25 == 0)
			{
				Imbue?.LingeringEffects(Projectile.Hitbox);
				SecondImbue?.LingeringEffects(Projectile.Hitbox);
			}

			Projectile.ai[1] += 1f / Projectile.extraUpdates;

			if (Main.myPlayer == Projectile.owner && Projectile.ai[1] >= 60)
			{
				Projectile.ai[1] = 0;
				Rectangle rect = new(Projectile.Hitbox.X, Projectile.Hitbox.Y - 500, Projectile.width, 500);
				rect = AOUtils.SimulateAOE(rect, Projectile.damage, Projectile.knockBack, Projectile, Projectile.DamageType, false, false);
				var amountmulti = 1f;
				if (Imbue is not null)
					amountmulti *= Imbue.AOScrollSize;
				if (SecondImbue is not null)
					amountmulti *= SecondImbue.AOScrollSize;
				for (int i = 0; i <= 10f * amountmulti; i++)
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
