using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;

namespace ArcaneOdyssey
{
	public class FirstCultistKill : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info) => !NPC.downedAncientCultist;
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => Language.GetOrRegister($"Mods.{nameof(ArcaneOdyssey)}.FirstCultistKillDescription", () => "First Lunatic Cultist Defeated").Value;
	}

	public class FirstMoonLordKill : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info) => !NPC.downedMoonlord;
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => Language.GetOrRegister($"Mods.{nameof(ArcaneOdyssey)}.FirstMoonLordKillDescription", () => "First Moon Lord Defeated").Value;
	}

	public class FirstEmpressKill : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info) => !NPC.downedEmpressOfLight;
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => Language.GetOrRegister($"Mods.{nameof(ArcaneOdyssey)}.FirstEmpressKillDescription", () => "First Empress of Light Defeated").Value;
	}

	public class FirstDayEmpressKill : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info) => !DownedBosses.downedEnragedEmpress;
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => Language.GetOrRegister($"Mods.{nameof(ArcaneOdyssey)}.FirstDayEmpressKillDescription", () => "First Enraged Empress of Light Defeated").Value;
	}

	public class NoShowNoConditon : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
        {
            if (info.npc is not null)
            {
                return !info.npc.SpawnedFromStatue;
            }
            return true;
        }
		public bool CanShowItemDropInUI() => false;
		public string GetConditionDescription() => "";
	}
}
