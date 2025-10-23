using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Cannons.Normal
{
	public class AshCannon : CannonSpell
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.alpha = (int)(225 * .07f);
		}

		public override void SetStaticDefaults() 
		{
			Main.projFrames[Type] = 7;
		}
	}
}
