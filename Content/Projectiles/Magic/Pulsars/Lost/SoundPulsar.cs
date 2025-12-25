using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Pulsars.Lost
{
	public class SoundPulsar : PulsarSpell
	{
		public override bool PreDraw(ref Color lightColor) => false;
		public override string Texture => Mod.Name + "/Backgrounds/Blank";
	}
}
