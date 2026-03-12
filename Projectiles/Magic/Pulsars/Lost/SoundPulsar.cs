using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;

namespace ArcaneOdyssey.Projectiles.Magic.Pulsars.Lost
{
	public class SoundPulsar : PulsarSpell
	{
		public override bool PreDraw(ref Color lightColor) => false;
		public override string Texture => AOUtils.BlankTexture;
	}
}
