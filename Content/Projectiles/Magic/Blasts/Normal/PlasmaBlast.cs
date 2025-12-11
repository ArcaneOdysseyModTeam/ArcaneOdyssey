using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Blasts.Normal
{
	public class PlasmaBlast : BlastSpell
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 4;
		}
	}
}
