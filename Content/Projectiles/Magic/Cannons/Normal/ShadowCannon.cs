using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Cannons.Normal
{
	public class ShadowCannon : CannonSpell
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 7;
		}
	}
}
