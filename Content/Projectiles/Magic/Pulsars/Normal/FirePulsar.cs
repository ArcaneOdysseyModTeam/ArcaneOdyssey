using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Pulsars
{
	public class FirePulsar : PulsarSpell
    {
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.alpha = 50;
		}

		public override void SetStaticDefaults() 
		{
			Main.projFrames[Type] = 4;
		}
	}
}
