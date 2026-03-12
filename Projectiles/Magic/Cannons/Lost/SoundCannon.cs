using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;

namespace ArcaneOdyssey.Projectiles.Magic.Cannons.Lost
{
	public class SoundCannon : CannonSpell
	{
		public override string Texture => AOUtils.BlankTexture;
		public override bool PreDraw(ref Color lightColor) => false;
	}
}
