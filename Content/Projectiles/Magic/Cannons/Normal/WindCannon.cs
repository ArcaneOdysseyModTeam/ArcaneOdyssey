using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Cannons.Normal
{
	public class WindCannon : CannonSpell
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
