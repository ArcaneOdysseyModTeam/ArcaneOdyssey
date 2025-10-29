using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Cannons.Lost
{
	public class DiamondCannon : CannonSpell
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.alpha = (int)(225 * .2f);
		}
		public override void SetStaticDefaults() {
			Main.projFrames[Type] = 4;
		}
	}
}
