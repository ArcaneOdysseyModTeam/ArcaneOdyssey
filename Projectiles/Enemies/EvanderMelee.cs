using ArcaneOdyssey.Projectiles.Base;

namespace ArcaneOdyssey.Projectiles.Enemies
{
	public class EvanderMelee : BaseProjectile
	{
		public override string Texture => AOUtils.BlankTexture;
		public override void SetDefaults()
		{
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 25;
			Projectile.hostile = true;
			Projectile.height = Projectile.width = 110;
			Projectile.tileCollide = false;
		}
		public override bool PreDraw(ref Color lightColor) => false;
	}
}
