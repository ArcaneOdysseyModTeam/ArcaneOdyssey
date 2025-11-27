using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Blasts.Lost
{
	public class LunarBlast : BlastSpell
	{
		public override void SetStaticDefaults() 
        {
			Main.projFrames[Type] = 5;
		}
	}
}
