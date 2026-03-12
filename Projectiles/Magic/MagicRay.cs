using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;

namespace ArcaneOdyssey.Projectiles.Magic
{
	public class MagicRay : MagicSpell
	{
		public override string Texture => AOUtils.BlankTexture;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 30; // thicker
			Projectile.extraUpdates = 100;
			Projectile.timeLeft = 85;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;
		}

		public override bool PreDraw(ref Color lightColor) => false;
	}
}
