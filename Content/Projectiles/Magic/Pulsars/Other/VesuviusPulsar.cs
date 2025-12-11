using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Pulsars.Other
{
	public class VesuviusPulsar : PulsarSpell
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 4;
		}

		public override void Rotate()
		{
			Projectile.rotation += 0.1f * Projectile.direction;
		}
	}
}
