using ArcaneOdyssey.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Projectiles.Magic.Arrays.Lost
{
	public class PrismArray : ArraySpell
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
		}
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 7;
		}
	}
}
