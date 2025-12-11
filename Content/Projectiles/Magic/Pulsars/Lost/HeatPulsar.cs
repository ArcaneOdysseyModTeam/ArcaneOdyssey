using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Pulsars.Lost
{
	public class HeatPulsar : PulsarSpell
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.alpha = (int)(225 * .4f);
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 7;
		}
	}
}
