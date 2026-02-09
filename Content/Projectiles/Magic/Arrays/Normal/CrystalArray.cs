using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Arrays.Normal
{
	public class CrystalArray : ArraySpell
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.alpha = 25;
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 8;
		}
	}
}
