using ArcaneOdyssey.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Projectiles.Magic.Cannons.Normal
{
	public class IceCannon : CannonSpell
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.alpha = (int)(225 * .3f);
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 4;
		}
	}
}
