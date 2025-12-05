using Terraria.Localization;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey
{
	public abstract class AODamageClass : DamageClass
	{
		public override LocalizedText DisplayName => Language.GetOrRegister(Mod.GetLocalizationKey($"DamageClasses.{Name}"), () => Mod.CustomLocalization($"DamageClasses.{Name}").Value);
	}

	public class Oracle : AODamageClass
	{
		public static readonly string InternalName = typeof(Oracle).Name;

		public override bool GetEffectInheritance(DamageClass damageClass)
		{
			return damageClass == Magic || damageClass == Summon;
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
			return false;
		}
	}

	/// <summary>
	/// Magic+Melee damage class
	/// </summary>
	public class Conjurer : AODamageClass
	{
		public static readonly string InternalName = typeof(Conjurer).Name;
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
	public class ConjurerNoSpeed : AODamageClass
	{
		public static readonly string InternalName = typeof(ConjurerNoSpeed).Name;
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
	public class Warlord : AODamageClass
	{
		public static readonly string InternalName = typeof(Warlord).Name;
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
	public class WarlordNoSpeed : AODamageClass
	{
		public static readonly string InternalName = typeof(WarlordNoSpeed).Name;
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
	/// Magic+TrueMelee damage class
	/// </summary>
	public class TrueConjurer : AODamageClass
	{
		public static readonly string InternalName = typeof(TrueConjurer).Name;
		public override bool GetEffectInheritance(DamageClass damageClass) => damageClass.CountsAsClass(TrueMelee());

		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass.CountsAsClass(TrueMelee()))
			{
				return MostInheritance;
			}
			if (damageClass == Magic)
			{
				return QuarterInheritance;
			}
			return base.GetModifierInheritance(damageClass);
		}

		public override bool GetPrefixInheritance(DamageClass damageClass) => damageClass.CountsAsClass(TrueMelee());
	}

	/// <summary>
	/// Magic+TrueMeleeNoSpeed damage class
	/// </summary>
	public class TrueConjurerNoSpeed : AODamageClass
	{
		public static readonly string InternalName = typeof(TrueConjurerNoSpeed).Name;
		public override bool GetEffectInheritance(DamageClass damageClass) => damageClass.CountsAsClass(TrueMelee());

		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass.CountsAsClass(TrueMelee()))
			{
				return MostInheritance with { attackSpeedInheritance = 1f };
			}
			if (damageClass == Magic)
			{
				return QuarterInheritance with { attackSpeedInheritance = 1f };
			}
			return base.GetModifierInheritance(damageClass);
		}

		public override bool GetPrefixInheritance(DamageClass damageClass) => damageClass.CountsAsClass(TrueMeleeNoSpeed());
	}

	/// <summary>
	/// Fighting Style+TrueMelee damage class
	/// </summary>
	public class TrueWarlord : AODamageClass
	{
		public static readonly string InternalName = typeof(TrueWarlord).Name;
		public override bool GetEffectInheritance(DamageClass damageClass) => damageClass.CountsAsClass(TrueMelee());

		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass == TrueMelee())
			{
				return WarlordInheritance;
			}
			return base.GetModifierInheritance(damageClass);
		}

		public override bool GetPrefixInheritance(DamageClass damageClass) => damageClass.CountsAsClass(TrueMelee());
	}

	/// <summary>
	/// Fighting Style+TrueMeleeNoSpeed damage class
	/// </summary>
	public class TrueWarlordNoSpeed : AODamageClass
	{
		public static readonly string InternalName = typeof(TrueWarlordNoSpeed).Name;
		public override bool GetEffectInheritance(DamageClass damageClass) => damageClass.CountsAsClass(TrueMelee());

		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass.CountsAsClass(TrueMelee()))
			{
				return WarlordInheritance with { attackSpeedInheritance = 1f };
			}
			return base.GetModifierInheritance(damageClass);
		}

		public override bool GetPrefixInheritance(DamageClass damageClass) => damageClass.CountsAsClass(TrueMeleeNoSpeed());
	}

	/// <summary>
	/// Magic+Ranged damage class
	/// </summary>
	public class RangedConjurer : AODamageClass
	{
		public static readonly string InternalName = typeof(RangedConjurer).Name;
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
	public class RangedWarlord : AODamageClass
	{
		public static readonly string InternalName = typeof(RangedWarlord).Name;
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
	public class RangedKnight : AODamageClass
	{
		public static readonly string InternalName = typeof(RangedKnight).Name;
		public override bool GetEffectInheritance(DamageClass damageClass) => damageClass == Ranged;

		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass == Ranged)
			{
				return MostInheritance;
			}    
			if (damageClass.Name == Oracle.InternalName)
			{
				return QuarterInheritance;
			}
			return base.GetModifierInheritance(damageClass);
		}

		public override bool GetPrefixInheritance(DamageClass damageClass) => damageClass == Ranged;
	}

	/// <summary>
	/// Spirit+TrueMelee damage class
	/// </summary>
	public class TrueKnight : AODamageClass
	{
		public static readonly string InternalName = typeof(TrueKnight).Name;
		public override bool GetEffectInheritance(DamageClass damageClass) => damageClass.CountsAsClass(TrueMelee());

		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass.CountsAsClass(TrueMelee()))
			{
				return MostInheritance;
			}
			if (damageClass.Name == Oracle.InternalName)
			{
				return QuarterInheritance;
			}
			return base.GetModifierInheritance(damageClass);
		}

		public override bool GetPrefixInheritance(DamageClass damageClass) => damageClass.CountsAsClass(TrueMelee());
	}

	/// <summary>
	/// Spirit+TrueMeleeNoSpeed damage class
	/// </summary>
	public class TrueKnightNoSpeed : AODamageClass
	{
		public static readonly string InternalName = typeof(TrueKnightNoSpeed).Name;
		public override bool GetEffectInheritance(DamageClass damageClass) => damageClass.CountsAsClass(TrueMelee());

		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass.CountsAsClass(TrueMelee()))
			{
				return MostInheritance with { attackSpeedInheritance = 1f };
			}
			if (damageClass.Name == Oracle.InternalName)
			{
				return QuarterInheritance with { attackSpeedInheritance = 1f };
			}
			return base.GetModifierInheritance(damageClass);
		}

		public override bool GetPrefixInheritance(DamageClass damageClass) => damageClass.CountsAsClass(TrueMeleeNoSpeed());
	}

	/// <summary>
	/// Spirit+Melee damage class
	/// </summary>
	public class Knight : AODamageClass
	{
		public static readonly string InternalName = typeof(Knight).Name;
		public override bool GetEffectInheritance(DamageClass damageClass) => damageClass.CountsAsClass(Melee);

		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass.CountsAsClass(Melee))
			{
				return MostInheritance;
			}
			if (damageClass.Name == Oracle.InternalName)
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
	public class KnightNoSpeed : AODamageClass
	{
		public static readonly string InternalName = typeof(ConjurerNoSpeed).Name;
		public override bool GetEffectInheritance(DamageClass damageClass) => damageClass.CountsAsClass(Melee);

		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
		{
			if (damageClass.CountsAsClass(Melee))
			{
				return MostInheritance with { attackSpeedInheritance = 1f };
			}
			if (damageClass.Name == Oracle.InternalName)
			{
				return QuarterInheritance with { attackSpeedInheritance = 1f };
			}
			return base.GetModifierInheritance(damageClass);
		}

		public override bool GetPrefixInheritance(DamageClass damageClass) => damageClass.CountsAsClass(MeleeNoSpeed);
	}
}
