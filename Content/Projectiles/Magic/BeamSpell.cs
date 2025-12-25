using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;

namespace ArcaneOdyssey.Content.Projectiles.Magic
{
	public class BeamSpell : MagicSpell
	{
		public override string Texture => Mod.Name + "/Backgrounds/Blank";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 4; // hitscan
			Projectile.extraUpdates = 100;
			Projectile.timeLeft = 75;
		}

		public override bool PreDraw(ref Color lightColor) => false;
	}
}
