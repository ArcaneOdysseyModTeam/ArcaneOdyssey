using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Projectiles.Base;

namespace ArcaneOdyssey.Content.Projectiles.Weapons
{
	public class BronzeTriastaProjectile : BaseSpearProjectile
	{
		public override AOItemTiers AOWeaponTier => AOItemTiers.Good;
		public override Debuff? ProjectileDebuff => Debuff.Create<CharredEffect>();
	}
}
