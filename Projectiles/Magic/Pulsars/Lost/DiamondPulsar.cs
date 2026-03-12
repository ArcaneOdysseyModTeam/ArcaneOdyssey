using ArcaneOdyssey.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Projectiles.Magic.Pulsars.Lost
{
	public class DiamondPulsar : PulsarSpell
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.alpha = (int)(225 * .2f);
		}
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 4;
		}
	}
}
