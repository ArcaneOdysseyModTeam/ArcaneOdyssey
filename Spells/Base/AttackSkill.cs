using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Spells.Base
{
	public abstract class AttackSkill : ModSkill
	{
		public abstract int Damage { get; }
		public virtual int ManaCost => 0;
		public virtual float Knockback => 4.5f;
		public virtual DamageClass DamageType => DamageClass.Magic;
		public virtual bool Channel => false;
		public sealed override SkillType SkillSlot => SkillType.Attack;
	}
}
