using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Arrays.Lost
{
	public class PrismArray : ArraySpell
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.alpha = (int)(225 * .4f);
		}
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 7;
		}
	}
}
