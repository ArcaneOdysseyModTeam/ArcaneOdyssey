using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Projectiles
{
	public class DeathCurse : AOBaseProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 8;
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
			offset = null;
		}

		private float? offset = null;

		public override void AI()
		{
			if (!Main.dedServ)
			{
				Dust spawnedDust = Dust.NewDustDirect(new Vector2(Projectile.position.X + (Projectile.width / 2f), Projectile.position.Y + (Projectile.height / 2f)), 1, 1, DustID.Wraith, (Main.rand.NextFloat() - 0.5f) * 10f, (Main.rand.NextFloat() - 0.5f) * 10f, Scale: 2f);
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Dust.NewDustDirect(new Vector2(Projectile.position.X + (Projectile.width / 2f), Projectile.position.Y + (Projectile.height / 2f)), 1, 1, DustID.Vortex, (Main.rand.NextFloat() - 0.5f) * 10f, (Main.rand.NextFloat() - 0.5f) * 10f, Scale: 2.6f);
				spawnedDust2.noGravity = true;
			}
			if (Projectile.Bottom.Y < 0 || Projectile.localAI[0] > 1000 || !Projectile.Hitbox.OnScreen())
			{
				Kill();
				return;
			}
			if (Projectile.frameCounter++ > 2)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Type])
				{
					Projectile.frame = 0;
				}
			}
			if (Projectile.localAI[0] > 50)
			{
				Projectile.velocity.Y += -23f / 30f;
				if (!offset.HasValue)
					offset = Main.rand.NextFloat() - 0.5f;
				Projectile.velocity.X += offset.Value * (13f / 15f);
			}
			else
			{
				Projectile.velocity *= 0.8f;
			}
			Projectile.localAI[0]++;
		}
	}
}