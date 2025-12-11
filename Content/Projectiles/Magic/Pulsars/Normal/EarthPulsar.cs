using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Pulsars.Normal
{
	public class EarthPulsar : PulsarSpell
	{
		public override void Rotate()
		{
			Projectile.rotation += 0.1f * Projectile.direction;
		}
	}
}
