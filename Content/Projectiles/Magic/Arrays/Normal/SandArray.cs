using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Arrays.Normal
{
	public class SandArray : ArraySpell
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 7;
		}
	}
}
