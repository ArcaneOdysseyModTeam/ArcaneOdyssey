using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;

namespace ArcaneOdyssey.Content.Projectiles.Magic
{
	public class LesserBeam : MagicSpell
	{
		public override string Texture => AOUtils.BlankTexture;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 4; // hitscan
			Projectile.extraUpdates = 100;
			Projectile.timeLeft = 40;
		}

		public override bool PreDraw(ref Color lightColor) => false;
	}
}
