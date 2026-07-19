using ArcaneOdyssey.Projectiles.Base;

namespace ArcaneOdyssey.Projectiles
{
	public class LeapFix : BaseProjectile
	{
		public override string Texture => AOUtils.BlankTexture;
		public override void AI()
		{
			Main.player[Projectile.owner].direction = (int)Projectile.ai[0];
			Kill();
		}
		public override bool PreDraw(ref Color lightColor) => false;
	}
}
