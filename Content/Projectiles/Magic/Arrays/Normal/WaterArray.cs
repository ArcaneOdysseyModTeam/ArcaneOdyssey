using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Arrays.Normal
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
