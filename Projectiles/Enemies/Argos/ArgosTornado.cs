using ArcaneOdyssey.Projectiles.Abilities;
using ArcaneOdyssey.Projectiles.Base;

namespace ArcaneOdyssey.Projectiles.Enemies.Argos
{
	public class ArgosTornado : BlockableProjectile
	{
		public override string Texture => AOUtils.GetTexture<Tempest>();
		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0]++;
				NetUpdate();
				Projectile.spriteDirection *= Main.rand.Next([-1, 1]);
			}
			Projectile.velocity = -(Vector2.UnitY * 3f);
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			if (Projectile.frameCounter++ >= 5)
			{
				Projectile.frameCounter = 0;
				if (Projectile.frame++ >= (Main.projFrames[Type] + 1))
				{
					Kill();
				}
			}
			Projectile.Opacity = .5f + (Projectile.frame / (float)Main.projFrames[Type]);
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.projFrames[Type] = 10;
		}

		public override float Size => 2.5f;
		public override bool? CanDamage() => false; // visual

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = 50;
			Projectile.width = 150;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.hostile = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 60;
			Projectile.penetrate = -1;
		}

		public override SpriteEffects FlippedMode => SpriteEffects.FlipHorizontally;

		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = Color.Orange;
			return base.PreDraw(ref lightColor);
		}
	}
}
