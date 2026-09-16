namespace ArcaneOdyssey.Skills.Base
{
	public abstract class DashSkill : ModSkill
	{
		public virtual int Damage => 0;
		public virtual float Knockback => 4.5f;
		public sealed override SkillType SkillSlot => SkillType.Dash;
	}
}
