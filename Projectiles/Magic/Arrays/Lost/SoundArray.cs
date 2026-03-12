using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;

namespace ArcaneOdyssey.Projectiles.Magic.Arrays.Lost
{
	public class SoundArray : ArraySpell
	{
		public override string Texture => AOUtils.BlankTexture;
		public override bool PreDraw(ref Color lightColor) => false;
	}
}
