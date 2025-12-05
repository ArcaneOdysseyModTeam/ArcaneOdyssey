using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Blasts.Lost
{
	public class SunBlast : BlastSpell
	{
		public override void SetStaticDefaults() 
		{
			Main.projFrames[Type] = 4;
		}
	}
}
