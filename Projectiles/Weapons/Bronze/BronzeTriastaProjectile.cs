using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Projectiles.Base;

namespace ArcaneOdyssey.Projectiles.Weapons.Bronze
{
	public class BronzeTriastaProjectile : BaseSpearProjectile
	{
		public override Debuff? ProjectileDebuff => Debuff.Create<CharredEffect>();

		public override float Size => 1.15f;
	}
}
