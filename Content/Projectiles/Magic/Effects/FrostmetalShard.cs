using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Effects
{
	public class FrostmetalShard : AOPlayerProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 3;
		}

		private float randomRotationOffset;

		public override float AOSize => 1.35f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.frame = Main.rand.Next(Main.projFrames[Type]);
			Projectile.width = Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;
			Projectile.timeLeft = 120;
			Projectile.DamageType = ModContent.GetInstance<ConjurerDamage>();
			randomRotationOffset = Main.rand.NextFloat(MathHelper.TwoPi);
		}

		public override void OnKill(int timeLeft)
		{
			if (!Main.dedServ)
			{
				for (int n = 0; n < 5; n++)
				{
					Dust spawnedDust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.SnowflakeIce, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
					spawnedDust.noGravity = true;
					SoundEngine.PlaySound(SoundID.Item27, Projectile.position, null);
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
			Projectile.rotation = Projectile.velocity.ToRotation() + randomRotationOffset;
		}
	}
}
