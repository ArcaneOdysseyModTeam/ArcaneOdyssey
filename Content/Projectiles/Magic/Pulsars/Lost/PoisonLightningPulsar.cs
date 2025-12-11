using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Pulsars.Lost
{
	public class PoisonLightningPulsar : PulsarSpell
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 6;
		}
	}
}
