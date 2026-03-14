using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Projectiles.Base;

namespace ArcaneOdyssey.Projectiles.Weapons
{
	public class BronzeTriastaProjectile : BaseSpearProjectile
	{
		public override AOItemTiers AOWeaponTier => AOItemTiers.Good;
		public override Debuff? ProjectileDebuff => Debuff.Create<CharredEffect>();

		public override float AOSize => 1.15f;
	}
}
