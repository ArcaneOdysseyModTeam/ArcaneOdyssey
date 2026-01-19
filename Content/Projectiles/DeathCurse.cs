using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles
{
	public class DeathCurse : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 8;
		}
		public override void SetDefaults()
		{
			Projectile.tileCollide = false;
			Projectile.width = Projectile.height = 100;
			Projectile.ignoreWater = true;
			Projectile.damage = 700;
			Projectile.hostile = true;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			offsett = null;
		}

		private float? offsett = null;

		public override void AI()
		{
			if (!Main.dedServ)
			{
				Dust spawnedDust = Dust.NewDustDirect(new Vector2(Projectile.position.X + (Projectile.width / 2f), Projectile.position.Y + (Projectile.height / 2f)), 1, 1, DustID.Wraith, (Main.rand.NextFloat() - 0.5f) * 10f, (Main.rand.NextFloat() - 0.5f) * 10f, 0, default, 2f);
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Dust.NewDustDirect(new Vector2(Projectile.position.X + (Projectile.width / 2f), Projectile.position.Y + (Projectile.height / 2f)), 1, 1, DustID.Vortex, (Main.rand.NextFloat() - 0.5f) * 10f, (Main.rand.NextFloat() - 0.5f) * 10f, 0, default, 2.6f);
				spawnedDust2.noGravity = true;
			}
			if (Projectile.Bottom.Y < 0 || Projectile.localAI[0] > 1000 || !Projectile.OnScreen())
			{
				Projectile.Kill();
			}
			if (Projectile.frameCounter++ > 2)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Projectile.type])
				{
					Projectile.frame = 0;
				}
			}
			if (Projectile.localAI[0] > 50)
			{
				Projectile.velocity.Y += -23f / 30f;
				if (!offsett.HasValue)
					offsett = Main.rand.NextFloat() - 0.5f;
				Projectile.velocity.X += offsett.Value * (13f / 15f);
			}
			else
			{
				Projectile.velocity *= 0.8f;
			}
			Projectile.localAI[0]++;
		}
	}
}