using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Cannons.Lost
{
	public class HeatCannon : CannonSpell
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.alpha = (int)(225 * .4f);
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 7;
		}
	}
}
