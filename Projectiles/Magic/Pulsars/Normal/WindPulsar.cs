using ArcaneOdyssey.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Projectiles.Magic.Pulsars.Normal
{
	public class WindPulsar : PulsarSpell
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 7;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.alpha = 60;
		}
	}
}
