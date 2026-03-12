using ArcaneOdyssey.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Projectiles.Magic.Blasts.Normal
{
	public class CrystalBlast : BlastSpell
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
