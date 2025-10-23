using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Projectiles.Weapons
{
	public class OldTridentProjectile : BaseSpearProjectile
	{
		public override AOItemTiers AOWeaponTier => AOItemTiers.Poor;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.DamageType = TrueMeleeNoSpeed();
		}
	}
}
