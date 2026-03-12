using ArcaneOdyssey.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Projectiles.Magic.Arrays.Normal
{
	public class WindArray : ArraySpell
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 7;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.alpha = 60;
		}
	}
}
