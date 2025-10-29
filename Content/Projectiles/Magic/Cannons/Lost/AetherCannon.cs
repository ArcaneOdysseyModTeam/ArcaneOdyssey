using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Cannons.Lost
{
	public class AetherCannon : CannonSpell
	{
		public override void SetStaticDefaults() {
			Main.projFrames[Type] = 7;
		}
	}
}
