using ArcaneOdyssey.Buffs.Minions;
using ArcaneOdyssey.Projectiles.Base;
using ArcaneOdyssey.Projectiles.Magic.Minions;

namespace ArcaneOdyssey.Projectiles.Relics.Minions
{
	public class SpiritMinion : Elemental, ISpiritProjectile
	{
		protected override int ProjID => ModContent.ProjectileType<MinionBlast>();
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.DamageType = DamageClass.Summon;
		}
		public override bool CheckActive()
		{
			if (Owner?.active == true)
			{
				if (Owner.DeadOrGhost)
				{
					Owner.ClearBuff(ModContent.BuffType<SpiritMinionBuff>());
					return false;
				}

				if (Owner.HasBuff(ModContent.BuffType<SpiritMinionBuff>()))
				{
					Projectile.timeLeft = 2;
				}

				return true;
			}
			return false;
		}
	}
}
