using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Arrays.Lost
{
	public class SoundArray : ArraySpell
	{
		public override string Texture => Mod.Name + "/Backgrounds/Blank";
		public override bool PreDraw(ref Color lightColor) => false;
	}
}
