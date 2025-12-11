using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles
{
	public class PowderExplosion : ModProjectile
	{
		public bool hasExploded = false;
		public override void SetDefaults()
		{
			Projectile.ai[0] = 0;
			Projectile.damage = 0;
			Projectile.height = Projectile.width = 100;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			hasExploded = false;
		}
		public override void AI()
		{
			Projectile.ai[0] += 1;
			if (Projectile.ai[0] >= 60)
			{
				if (!hasExploded)
				{
					float AOScrollSize = 1f;
					Projectile.damage = (int)Projectile.ai[1];
					SoundEngine.PlaySound(SoundID.Item14, Projectile.position, null);
					for (int n = 0; n < 8; n++)
					{
						Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(Projectile.position.X + (Projectile.width / 2f), Projectile.position.Y + (Projectile.height / 2f)), 1, 1, DustID.Pixie, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 3f)];
						spawnedDust.noGravity = true;
						Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(Projectile.position.X + (Projectile.width / 2f), Projectile.position.Y + (Projectile.height / 2f)), 1, 1, DustID.Pixie, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 3f)];
						spawnedDust2.noGravity = true;
						Dust spawnedDust3 = Main.dust[Dust.NewDust(new Vector2(Projectile.position.X + (Projectile.width / 2f), Projectile.position.Y + (Projectile.height / 2f)), 1, 1, DustID.Ash, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 4f)];
						spawnedDust3.noGravity = true;
					}
				}
				hasExploded = true;
				if (Projectile.ai[0] >= 120)
				{
					Projectile.Kill();
				}
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			return false;
		}
	}
}
