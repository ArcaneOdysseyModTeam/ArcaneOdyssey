using ArcaneOdyssey.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Projectiles.Magic.Cannons.Normal
{
	public class LightningCannon : CannonSpell
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 6;
		}
	}
}
