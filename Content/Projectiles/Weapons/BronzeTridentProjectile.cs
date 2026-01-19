using ArcaneOdyssey.Content.Items.Weapons.Bronze;
using ArcaneOdyssey.Content.Projectiles.Base;

namespace ArcaneOdyssey.Content.Projectiles.Weapons
{
	public class BronzeTridentProjectile : BaseSpearProjectile
	{
		public override string Texture => typeof(BronzeTrident).Texture();
		public override AOItemTiers AOWeaponTier => AOItemTiers.Average;
	}
}
