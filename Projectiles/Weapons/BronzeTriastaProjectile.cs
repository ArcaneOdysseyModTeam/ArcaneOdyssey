using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Projectiles.Base;

namespace ArcaneOdyssey.Projectiles.Weapons
{
	public class BronzeTriastaProjectile : BaseSpearProjectile
	{
		public override ItemTiers AOWeaponTier => ItemTiers.Good;
		public override Debuff? ProjectileDebuff => Debuff.Create<CharredEffect>();

		public override float Size => 1.15f;
	}
}
