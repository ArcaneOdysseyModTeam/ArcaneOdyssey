using ArcaneOdyssey.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Projectiles.Magic.Arrays.Lost
{
	public class DiamondArray : ArraySpell
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.alpha = (int)(225 * .2f);
		}
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 4;
		}
	}
}
