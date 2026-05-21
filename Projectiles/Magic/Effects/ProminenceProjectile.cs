using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Projectiles.Magic.Effects
{
	public class ProminenceProjectile : PlayerProjectile
	{
		public override Debuff? ProjectileDebuff => Debuff.Create<Melting>(120);

		private Vector2 originPos;
		private int timeAlive = 0;
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 3;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.tileCollide = false;
			Projectile.width = Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
			Projectile.timeLeft = 6 * 60;
		}

		public override void AI()
		{
			if (timeAlive < 2)
			{
				originPos = Projectile.Center;
			}
			if (Projectile.wet && !(Projectile.lavaWet || Projectile.honeyWet || Projectile.shimmerWet))
			{
				Kill();
				return;
			}

			timeAlive++;
			if (timeAlive == 29)
			{
				Projectile.velocity += new Vector2(Main.rand.NextFloat() * 0.5f, Main.rand.NextFloat() * 0.5f) * 10;
			}
			if (timeAlive > 30)
			{
				Projectile.velocity += (originPos - Projectile.Center).SafeNormalize(Vector2.Zero) * 0.4f;
			}
			Animate();
			Lighting.AddLight(Projectile.Center, new Vector3(2, 1, 0) * Projectile.scale);
			Dust.NewDust(Projectile.Center, 0, 0, DustID.Torch);
		}

		private void Animate()
		{
			if (Projectile.frameCounter++ > 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Type])
				{
					Projectile.frame = 0;
				}
			}
		}
	}
}