using ArcaneOdyssey.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Projectiles.Magic.Cannons.Lost
{
	public class PrismCannon : CannonSpell
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
		}
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 7;
		}
	}
}
