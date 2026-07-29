using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Projectiles.Base;
using ArcaneOdyssey.Projectiles.Relics;
using Terraria.Audio;

namespace ArcaneOdyssey.Projectiles.Enemies
{
	public class EliusSpear : BaseProjectile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 500;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.height = Projectile.width = 70;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
		}
		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
		}
	}
}
