using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Blasts.Normal
{
	public class AshBlast : BlastSpell
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.alpha = (int)(225 * .07f);
		}

		public override void SetStaticDefaults() 
		{
			Main.projFrames[Type] = 7;
		}
	}
}
