using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Cannons.Lost
{
	public class StormCannon : CannonSpell
	{
		public override void SetStaticDefaults() 
		{
			Main.projFrames[Type] = 6;
		}
	}
}
