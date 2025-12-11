using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Pulsars.Normal
{
	public class ShadowPulsar : PulsarSpell
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 7;
		}
	}
}
