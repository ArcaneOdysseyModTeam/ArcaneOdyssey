using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Projectiles.Magic.MagicEffects
{
	public class FrostmetalShard : AOPlayerProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 3;
		}

		public override void SetDefaults()
		{
			Projectile.frame = Main.rand.Next(Main.projFrames[Type]);
			Projectile.width = Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;
			Projectile.timeLeft = 120;
			BaseScale = 1.35f;
		}

		public override void OnKill(int timeLeft)
		{
			if (!Main.dedServ)
			{
				for (int n = 0; n < 5; n++)
				{
					Dust spawnedDust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.SnowflakeIce, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
					spawnedDust.noGravity = true;
					Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Ice, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 2f);
					Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Mercury, 2f * (Main.rand.NextFloat() - 0.5f), 2f * (Main.rand.NextFloat() - 0.5f), 0, default, 1f);
				}
			}
		}

		public override void AI()
		{
			Projectile.velocity.Y += 0.13f;
			if (Projectile.velocity.Y > 16f)
			{
				Projectile.velocity.Y = 16f;
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
		}
	}
}
