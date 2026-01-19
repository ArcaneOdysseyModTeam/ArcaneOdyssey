using Terraria.Localization;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey
{
	public abstract class AODamageClass : DamageClass
	{
		public override LocalizedText DisplayName => Language.GetOrRegister(Mod.GetLocalizationKey($"DamageClasses.{Name}"), () => Mod.CustomLocalization($"DamageClasses.{Name}").Value);
	}

	public class OracleDamage : AODamageClass
	{
		public static readonly string InternalName = typeof(OracleDamage).Name;

		public override bool GetEffectInheritance(DamageClass damageClass)
		{
			return damageClass == Magic;
		}

		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass == Summon)
			{
				return MostInheritance;
			}
			if (damageClass == Magic)
			{
				return QuarterInheritance;
			}
			return base.GetModifierInheritance(damageClass);
		}

		public override bool GetPrefixInheritance(DamageClass damageClass)
		{
			return damageClass == Magic;
		}
	}

	/// <summary>
	/// Magic+Melee damage class
	/// </summary>
	public class ConjurerDamage : AODamageClass
	{
		public static readonly string InternalName = typeof(ConjurerDamage).Name;
		public override bool GetEffectInheritance(DamageClass damageClass) => damageClass.CountsAsClass(Melee);

		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass.CountsAsClass(Melee))
			{
				return MostInheritance;
			}
			if (damageClass == Magic)
			{
				return QuarterInheritance;
			}
			return base.GetModifierInheritance(damageClass);
		}

		public override bool GetPrefixInheritance(DamageClass damageClass) => damageClass.CountsAsClass(Melee);
	}

	/// <summary>
	/// Magic+MeleeNoSpeed damage class
	/// </summary>
	public class ConjurerNoSpeedDamage : AODamageClass
	{
		public static readonly string InternalName = typeof(ConjurerNoSpeedDamage).Name;
		public override bool GetEffectInheritance(DamageClass damageClass) => damageClass.CountsAsClass(Melee);

		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass.CountsAsClass(Melee))
			{
				return MostInheritance with { attackSpeedInheritance = 1f };
			}
			if (damageClass == Magic)
			{
				return QuarterInheritance with { attackSpeedInheritance = 1f };
			}
			return base.GetModifierInheritance(damageClass);
		}

		public override bool GetPrefixInheritance(DamageClass damageClass) => damageClass.CountsAsClass(MeleeNoSpeed);
	}

	/// <summary>
	/// Fighting Style+Melee damage class
	/// </summary>
	public class WarlordDamage : AODamageClass
	{
		public static readonly string InternalName = typeof(WarlordDamage).Name;
		public override bool GetEffectInheritance(DamageClass damageClass) => damageClass.CountsAsClass(Melee);

		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass.CountsAsClass(Melee))
			{
				return WarlordInheritance;
			}
			return base.GetModifierInheritance(damageClass);
		}

		public override bool GetPrefixInheritance(DamageClass damageClass) => damageClass.CountsAsClass(Melee);
	}

	/// <summary>
	/// Fighting Style+MeleeNoSpeed damage class
	/// </summary>
	public class WarlordNoSpeedDamage : AODamageClass
	{
		public static readonly string InternalName = typeof(WarlordNoSpeedDamage).Name;
		public override bool GetEffectInheritance(DamageClass damageClass) => damageClass.CountsAsClass(Melee);

		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass.CountsAsClass(Melee))
			{
				return WarlordInheritance with { attackSpeedInheritance = 1f };
			}
			return base.GetModifierInheritance(damageClass);
		}

		public override bool GetPrefixInheritance(DamageClass damageClass) => damageClass.CountsAsClass(MeleeNoSpeed);
	}

	/// <summary>
	/// Magic+Ranged damage class
	/// </summary>
	public class RangedConjurerDamage : AODamageClass
	{
		public static readonly string InternalName = typeof(RangedConjurerDamage).Name;
		public override bool GetEffectInheritance(DamageClass damageClass) => damageClass == Ranged;

		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass == Ranged)
			{
				return MostInheritance;
			}
			if (damageClass == Magic)
			{
				return QuarterInheritance;
			}
			return base.GetModifierInheritance(damageClass);
		}

		public override bool GetPrefixInheritance(DamageClass damageClass) => damageClass == Ranged;
	}

	/// <summary>
	/// Spirit+Ranged damage class
	/// </summary>
	public class RangedWarlordDamage : AODamageClass
	{
		public static readonly string InternalName = typeof(RangedWarlordDamage).Name;
		public override bool GetEffectInheritance(DamageClass damageClass) => damageClass == Ranged;

		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass == Ranged)
			{
				return WarlordInheritance;
			}
			return base.GetModifierInheritance(damageClass);
		}

		public override bool GetPrefixInheritance(DamageClass damageClass) => damageClass == Ranged;
	}

	/// <summary>
	/// Spirit+Ranged damage class
	/// </summary>
	public class RangedKnightDamage : AODamageClass
	{
		public static readonly string InternalName = typeof(RangedKnightDamage).Name;
		public override bool GetEffectInheritance(DamageClass damageClass) => damageClass == Ranged;

		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass == Ranged)
			{
				return MostInheritance;
			}
			if (damageClass.Name == OracleDamage.InternalName)
			{
				return QuarterInheritance;
			}
			return base.GetModifierInheritance(damageClass);
		}

		public override bool GetPrefixInheritance(DamageClass damageClass) => damageClass == Ranged;
	}

	/// <summary>
	/// Spirit+Melee damage class
	/// </summary>
	public class KnightDamage : AODamageClass
	{
		public static readonly string InternalName = typeof(KnightDamage).Name;
		public override bool GetEffectInheritance(DamageClass damageClass) => damageClass.CountsAsClass(Melee);

		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass.CountsAsClass(Melee))
			{
				return MostInheritance;
			}
			if (damageClass.Name == OracleDamage.InternalName)
			{
				return QuarterInheritance;
			}
			return base.GetModifierInheritance(damageClass);
		}

		public override bool GetPrefixInheritance(DamageClass damageClass) => damageClass.CountsAsClass(Melee);
	}

	/// <summary>
	/// Spirit+MeleeNoSpeed damage class
	/// </summary>
	public class KnightNoSpeedDamage : AODamageClass
	{
		public static readonly string InternalName = typeof(ConjurerNoSpeedDamage).Name;
		public override bool GetEffectInheritance(DamageClass damageClass) => damageClass.CountsAsClass(Melee);

		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass.CountsAsClass(Melee))
			{
				return MostInheritance with { attackSpeedInheritance = 1f };
			}
			if (damageClass.Name == OracleDamage.InternalName)
			{
				return QuarterInheritance with { attackSpeedInheritance = 1f };
			}
			return base.GetModifierInheritance(damageClass);
		}

		public override bool GetPrefixInheritance(DamageClass damageClass) => damageClass.CountsAsClass(MeleeNoSpeed);
	}

	public class WarlockDamage : ConjurerDamage
	{
		public new static readonly string InternalName = typeof(WarlockDamage).Name;
	}

	public class JuggernautDamage : KnightDamage
	{
		public new static readonly string InternalName = typeof(JuggernautDamage).Name;
	}

	public class PaladinDamage : AODamageClass
	{
		public static readonly string InternalName = typeof(PaladinDamage).Name;

		public override bool GetEffectInheritance(DamageClass damageClass)
		{
			return damageClass == Magic;
		}

		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass.Name == OracleDamage.InternalName)
			{
				return HalfInheritance;
			}
			if (damageClass == Magic)
			{
				return HalfInheritance;
			}
			return base.GetModifierInheritance(damageClass);
		}

		public override bool GetPrefixInheritance(DamageClass damageClass)
		{
			return damageClass == Magic;
		}
	}
}
