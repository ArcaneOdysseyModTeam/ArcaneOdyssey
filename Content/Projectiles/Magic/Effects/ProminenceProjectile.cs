using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Effects
{
	public class ProminenceProjectile : AOPlayerProjectile
	{
		public override AODebuffRequirement? Debuff => new(BuffID.OnFire3, 120);

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
			originPos = Projectile.Center;
		}

		public override void AI()
		{
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
			Lighting.AddLight(Projectile.Center, 2, 1, 0);
			Dust.NewDust(Projectile.Center, 0, 0, DustID.Torch, 0, 0, 0, default, 1);
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