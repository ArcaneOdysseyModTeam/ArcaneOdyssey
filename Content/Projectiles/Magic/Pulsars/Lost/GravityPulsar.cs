using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Pulsars.Lost
{
	public class GravityPulsar : PulsarSpell
    {
		public override void SetStaticDefaults() {
			Main.projFrames[Type] = 4;
		}
	}
}
