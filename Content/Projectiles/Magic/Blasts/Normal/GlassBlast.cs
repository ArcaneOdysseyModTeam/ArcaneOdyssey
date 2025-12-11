using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Blasts.Normal
{
	public class GlassBlast : BlastSpell
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.alpha = (int)(225 * .4f);
		}
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 8;
		}
	}
}
