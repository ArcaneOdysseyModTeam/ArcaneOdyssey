using ArcaneOdyssey.Items.Weapons.Bronze;
using ArcaneOdyssey.Projectiles.Base;

namespace ArcaneOdyssey.Projectiles.Weapons
{
	public class BronzeTridentProjectile : BaseSpearProjectile
	{
		public override string Texture => AOUtils.GetTexture<BronzeTrident>();
		public override AOItemTiers AOWeaponTier => AOItemTiers.Average;
	}
}
