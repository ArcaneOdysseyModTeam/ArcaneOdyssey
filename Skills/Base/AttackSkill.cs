using ArcaneOdyssey.Imbues.Base;
using Terraria.Audio;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Skills.Base
{
	public abstract class AttackSkill : ModSkill
	{
		public virtual int Time => 30;
		public abstract int Damage { get; }
		public abstract int Shoot { get; }
		public virtual int ManaCost => 0;
		public virtual float Knockback => 4.5f;
		public virtual bool Channel => false;
		public virtual float Speed => 1f;
		public virtual SoundStyle? ExtraSound => null;

		public sealed override SkillType SkillSlot => SkillType.Attack;

		public abstract bool Attack(Player player, Imbuable imbue, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback);

		public virtual void AttackStats(Player player, Imbuable imbue, ref Vector2 position, ref Vector2 velocity, ref int damage, ref float knockback) { }

		public sealed override void Activate(Player player, Imbuable imbue) { }

		public static bool AltUsing => AOKeybinds.AltSkillUse.Current;

		public virtual int UseStyleID => ItemUseStyleID.Rapier;

		public virtual void ModifyManaCost(Player player, ref float reduce, ref float mult) { }
	}
}
