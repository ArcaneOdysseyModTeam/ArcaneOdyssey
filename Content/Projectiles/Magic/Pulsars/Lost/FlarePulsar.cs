using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Pulsars.Lost
{
	public class FlarePulsar : PulsarSpell
    {
		public override void SetStaticDefaults() {
			Main.projFrames[Type] = 3;
		}
	}
}
