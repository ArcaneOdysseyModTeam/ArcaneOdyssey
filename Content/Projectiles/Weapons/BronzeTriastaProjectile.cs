using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Weapons
{
	public class BronzeTriastaProjectile : BaseSpearProjectile
	{
		public override AOItemTiers AOWeaponTier => AOItemTiers.Good;
        public override AODebuffRequirement? Debuff => new(ModContent.BuffType<CharredEffect>(), 60 * 10);
	}
}
