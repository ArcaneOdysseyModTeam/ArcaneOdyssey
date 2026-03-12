using ArcaneOdyssey.Projectiles.Base;

namespace ArcaneOdyssey.Projectiles.Magic.Pulsars.Lost
{
	public class ThreadPulsar : PulsarSpell
	{
		public override void Rotate()
		{
			Projectile.rotation += 0.1f * Projectile.direction;
		}
	}
}
