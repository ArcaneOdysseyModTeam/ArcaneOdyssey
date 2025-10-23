using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Pulsars.Ancient
{
	public class IonPulsar : PulsarSpell
    {
		public override void SetStaticDefaults() 
        {
			Main.projFrames[Type] = 4;
		}
	}
}
