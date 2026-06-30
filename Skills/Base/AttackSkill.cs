using ArcaneOdyssey.Imbues.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

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
		public virtual float Speed => 0f;
		public sealed override SkillType SkillSlot => SkillType.Attack;

		public abstract bool Attack(Player player, Imbuable imbue, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback);

		public sealed override void Activate(Player player, Imbuable imbue) { }
	}
}
