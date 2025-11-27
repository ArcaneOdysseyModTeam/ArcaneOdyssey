using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Blasts.Lost
{
	public class PlantBlast : BlastSpell
	{
		public override void SetStaticDefaults() 
        {
			Main.projFrames[Type] = 2;
		}
	}
}
