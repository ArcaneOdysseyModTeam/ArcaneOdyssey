using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Pulsars
{
	public class MetalPulsar : PulsarSpell
	{
		public override void SetStaticDefaults() 
        {
			Main.projFrames[Type] = 4;
		}
	}
}
