using ArcaneOdyssey.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Projectiles.Magic.Pulsars.Normal
{
	public class WaterPulsar : PulsarSpell
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.alpha = 50;
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 5;
		}
	}
}
