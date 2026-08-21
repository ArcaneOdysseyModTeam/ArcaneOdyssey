using ArcaneOdyssey.DamageClasses.Base;

namespace ArcaneOdyssey.DamageClasses
{
	public class RangedArcaniumDamageClass : BaseDamageClass
	{
		public override bool GetEffectInheritance(DamageClass damageClass) => damageClass is MagicDamageClass or RangedDamageClass or GenericDamageClass;

		public override StatInheritanceData GetModifierInheritance(DamageClass damageClass) => GetEffectInheritance(damageClass) ? StatInheritanceData.Full : StatInheritanceData.None;

		public override bool GetPrefixInheritance(DamageClass damageClass) => damageClass == Ranged;
	}
}
