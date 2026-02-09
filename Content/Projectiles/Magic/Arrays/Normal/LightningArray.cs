using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Arrays.Normal
{
	public class LightningArray : ArraySpell
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 6;
		}
	}
}
