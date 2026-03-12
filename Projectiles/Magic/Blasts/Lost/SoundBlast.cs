using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;

namespace ArcaneOdyssey.Projectiles.Magic.Blasts.Lost
{
	public class SoundBlast : BlastSpell
	{
		public override bool PreDraw(ref Color lightColor) => false;
		public override string Texture => AOUtils.BlankTexture;
	}
}
