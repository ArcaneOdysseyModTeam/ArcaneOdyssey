using ArcaneOdyssey.Content.Items.Weapons.Bronze;
using ArcaneOdyssey.Content.Projectiles.Base;

namespace ArcaneOdyssey.Content.Projectiles.Weapons
{
	public class BronzeTridentProjectile : BaseSpearProjectile
	{
		public override string Texture => AOUtils.GetTexture<BronzeTrident>();
		public override AOItemTiers AOWeaponTier => AOItemTiers.Average;
	}
}
