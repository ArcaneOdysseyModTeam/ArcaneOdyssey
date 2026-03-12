using ArcaneOdyssey.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Projectiles.Magic.Pulsars.Lost
{
	public class PrismPulsar : PulsarSpell
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
		}
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 7;
		}
	}
}
