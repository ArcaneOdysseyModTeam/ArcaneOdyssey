using ArcaneOdyssey.Skills.Base;
using ArcaneOdyssey.Skills.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.Base
{
	public abstract class FightingStyle : Imbuable
	{
		public override void Load()
		{
			base.Load();
			ModTypeLookup<FightingStyle>.Register(this);
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DamageType = DamageClass.Melee;
		}

		public override AttackSkill DefaultAttack => ModContent.GetInstance<StrikeSkill>();
	}
}
