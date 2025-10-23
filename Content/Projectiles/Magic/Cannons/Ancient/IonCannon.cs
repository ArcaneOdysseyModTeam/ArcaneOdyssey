using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Cannons
{
	public class IonCannon : CannonSpell
	{
		public override void SetStaticDefaults() 
        {
			Main.projFrames[Type] = 4;
		}
	}
}
