using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Items.Base;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class PhoenixMagic : AOMagic
	{
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override float AOImbueDamage => 1f;
		public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1f;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<PhoenixHealing>(), 60 * 10),];

		
	}
}
