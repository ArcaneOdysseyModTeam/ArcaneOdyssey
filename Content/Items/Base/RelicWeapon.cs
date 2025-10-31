using Terraria.Localization;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Base
{
	public abstract class RelicWeapon : AOBaseItem
	{
		public abstract int AOValue { get; }

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DamageType = ModContent.GetInstance<SpiritDamage>();
			Item.noUseGraphic = true;
			Item.noMelee = true; // could add a virtual bool to toggle this later
			Item.value = GalleonToCopper(AOValue);
		}
	}

	public class SpiritDamage : DamageClass
	{
		public override LocalizedText DisplayName => Mod.CustomLocalization("SpiritDamage");

		public override bool GetEffectInheritance(DamageClass damageClass)
		{
			return damageClass == MagicSummonHybrid || damageClass == Magic || damageClass == Summon || damageClass == SummonMeleeSpeed;
		}

		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass == Summon)
			{
				return new StatInheritanceData(0.75f, 0.75f, 0.75f, 0.75f, 0.75f);
			}
			if (damageClass == Magic)
			{
				return new StatInheritanceData(0.25f, 0.25f, 0.25f, 0.25f, 0.25f);
			}
			return base.GetModifierInheritance(damageClass);
		}

		public override bool GetPrefixInheritance(DamageClass damageClass)
		{
			return damageClass == Magic || damageClass == Summon;
		}
	}
}
