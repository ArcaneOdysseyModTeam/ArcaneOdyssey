using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Pulsars.Normal
{
	public class LightningPulsar : PulsarSpell
    {
		public override void SetStaticDefaults() 
        {
			Main.projFrames[Type] = 6;
		}
	}
}
