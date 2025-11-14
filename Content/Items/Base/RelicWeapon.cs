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
			Item.noUseGraphic = true; // could add a virtual bool to toggle this later
            Item.noMelee = true;
			Item.value = GalleonToCopper(AOValue);
		}
	}

	public class SpiritDamage : DamageClass
	{
        public static readonly string InternalName = typeof(SpiritDamage).Name;
		public override LocalizedText DisplayName => Mod.CustomLocalization("SpiritDamage");

		public override bool GetEffectInheritance(DamageClass damageClass)
		{
			return damageClass == Magic || damageClass == Summon;
		}

		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass == Summon)
			{
				return ThreeQuartersInheritance;
			}
			if (damageClass == Magic)
			{
				return QuarterInheritance;
			}
			return base.GetModifierInheritance(damageClass);
		}

		public override bool GetPrefixInheritance(DamageClass damageClass)
        {
            return false;
        }
	}
}
