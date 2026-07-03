namespace ArcaneOdyssey.Skills.Base
{
	public abstract class PassiveSkill : ModSkill
	{
		public abstract int Length { get; }
		public sealed override SkillType SkillSlot => SkillType.Passive;
	}
}
