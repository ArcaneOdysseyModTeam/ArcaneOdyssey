using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Blasts.Normal
{
	public class EarthBlast : BlastSpell
	{
		public override void Rotate()
		{
			Projectile.rotation += 0.1f * Projectile.direction;
		}
	}
}
