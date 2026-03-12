using ArcaneOdyssey.Projectiles.Base;

namespace ArcaneOdyssey.Projectiles.Magic.Blasts.Lost
{
	public class ThreadBlast : BlastSpell
	{
		public override void Rotate()
		{
			Projectile.rotation += 0.1f * Projectile.direction;
		}
	}
}
