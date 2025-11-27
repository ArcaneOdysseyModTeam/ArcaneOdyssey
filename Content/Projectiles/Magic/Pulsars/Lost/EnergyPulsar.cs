using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Pulsars.Lost
{
	public class EnergyPulsar : PulsarSpell
    {
		public override void SetStaticDefaults() 
		{
			Main.projFrames[Type] = 3;
		}
	}
}
