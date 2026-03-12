using ArcaneOdyssey.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Projectiles.Magic.Arrays.Normal
{
	public class WaterArray : ArraySpell
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.alpha = 50;
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 5;
		}
	}
}
