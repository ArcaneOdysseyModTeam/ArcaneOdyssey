using ArcaneOdyssey.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Projectiles.Magic.Blasts.Lost
{
	public class DiamondBlast : BlastSpell
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
