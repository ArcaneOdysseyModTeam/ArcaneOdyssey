using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Projectiles.Base;
using ArcaneOdyssey.Projectiles.Relics;
using Terraria.Audio;

namespace ArcaneOdyssey.Projectiles.Enemies
{
	public class EliusArrowStorm : BaseProjectile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 500;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.height = Projectile.width = 30;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
		}
		public override void AI()
		{
			if(Projectile.ai[0] == 0)
			{
				Projectile.velocity = Projectile.Center.DirectionTo(new Vector2(Projectile.ai[1],Projectile.ai[2])).SafeNormalize() * 20f;
				if(Projectile.Center.Distance(new Vector2(Projectile.ai[1],Projectile.ai[2])) < 25f)
				{
					Projectile.Center = new Vector2(Projectile.ai[1],Projectile.ai[2]);
					for(int i = -5;i<=5;i++) {
						Projectile.NewProjectile(Projectile.GetSource_FromThis(),Projectile.position,new Vector2(i*3f,0f),ModContent.ProjectileType<EliusArrowStorm>(),Projectile.damage,0f,-1,1f);
					}
					Projectile.Kill();
				}
			} else
			{
				Projectile.velocity.Y += 0.4f;
				Projectile.velocity *= 0.98f;
			}
			Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
		}
	}
}
