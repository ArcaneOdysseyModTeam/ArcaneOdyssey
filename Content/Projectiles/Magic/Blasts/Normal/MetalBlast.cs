using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Blasts.Normal
{
	public class MetalBlast : BlastSpell
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 4;
		}
	}
}
