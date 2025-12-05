using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Pulsars.Normal
{
	public class IcePulsar : PulsarSpell
    {
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.alpha = (int)(225 * .3f);
		}

		public override void SetStaticDefaults() 
        {
			Main.projFrames[Type] = 4;
		}
	}
}
