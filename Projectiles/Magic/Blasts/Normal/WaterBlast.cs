using ArcaneOdyssey.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Projectiles.Magic.Blasts.Normal
{
	public class WaterBlast : BlastSpell
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 5;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.alpha = 50;
		}
	}
}
