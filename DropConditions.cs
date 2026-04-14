using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArcaneOdyssey
{
	public class FirstCultistKill : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info) => !NPC.downedAncientCultist;
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => Language.GetOrRegister($"Mods.{ArcaneOdysseyMod.InternalName}.DropConditions.FirstCultistKillDescription", () => "First Lunatic Cultist Defeated").Value;
	}

	public class FirstGolemKill : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info) => !NPC.downedGolemBoss;
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => Language.GetOrRegister($"Mods.{ArcaneOdysseyMod.InternalName}.DropConditions.FirstGolemKillDescription", () => "First Golem Defeated").Value;
	}

	public class FirstMoonLordKill : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info) => !NPC.downedMoonlord;
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => Language.GetOrRegister($"Mods.{ArcaneOdysseyMod.InternalName}.DropConditions.FirstMoonLordKillDescription", () => "First Moon Lord Defeated").Value;
	}

	public class FirstEmpressKill : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info) => !NPC.downedEmpressOfLight;
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => Language.GetOrRegister($"Mods.{ArcaneOdysseyMod.InternalName}.DropConditions.FirstEmpressKillDescription", () => "First Empress of Light Defeated").Value;
	}

	public class FirstEvanderKill : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info) => !DownedBosses.DownedEvander;
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => Language.GetOrRegister($"Mods.{ArcaneOdysseyMod.InternalName}.DropConditions.FirstEvanderKillDescription", () => "First Evander Defeated").Value;
	}
	public class NotFirstEvanderKill : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info) => DownedBosses.DownedEvander;
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => Language.GetOrRegister($"Mods.{ArcaneOdysseyMod.InternalName}.DropConditions.NotFirstEvanderKillDescription", () => "Following Evanders Defeated").Value;
	}

	public class FirstDayEmpressKill : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info) => !DownedBosses.DownedEnragedEmpress && info.npc.AI_120_HallowBoss_IsGenuinelyEnraged();
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => Language.GetOrRegister($"Mods.{ArcaneOdysseyMod.InternalName}.DropConditions.FirstDayEmpressKillDescription", () => "First Enraged Empress of Light Defeated").Value;
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

	public class DownedAllMechBossesFirstTime : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (NPC.downedMechBoss1 && NPC.downedMechBoss2)
				return !NPC.downedMechBoss3;

			if (NPC.downedMechBoss1 && NPC.downedMechBoss3)
				return (!NPC.downedMechBoss2) && !AOUtils.BothTwinsAlive;

			if (NPC.downedMechBoss3 && NPC.downedMechBoss2)
				return !NPC.downedMechBoss1;

			return false;
		}

		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => Language.GetOrRegister($"Mods.{ArcaneOdysseyMod.InternalName}.DropConditions.FirstMechBossesKillDescription", () => "First Mechanical Trio Defeated").Value;
	}

	public class KilledABoss : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info) => AOUtils.BossesKilled > 0;

		public bool CanShowItemDropInUI() => true;

		public string GetConditionDescription() => Language.GetOrRegister($"Mods.{ArcaneOdysseyMod.InternalName}.DropConditions.KilledABossDescription", () => "Defeated at least one strong foe").Value;
	}

	public class MultiDropHelper(int itemID, int denominator = 1, int minQuantity = 1, int maxQuantity = 1, int numerator = 1) : CommonDrop(itemID, denominator, minQuantity, maxQuantity, numerator)
	{
		public override ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			ItemDropAttemptResult result = default;
			if (info.rng.Next(chanceDenominator) < chanceNumerator)
			{
				if (!(itemId <= 0 || itemId >= ItemLoader.ItemCount))
				{
					if (Main.dedServ)
					{
						var item = Item.NewItem(info.npc.GetSource_Loot(), info.npc.Center, itemId, 1, true, -1);
						Main.timeItemSlotCannotBeReusedFor[item] = 60 * 60 * 5;
						foreach (var player in Main.ActivePlayers)
							NetMessage.SendData(MessageID.InstancedItem, player.whoAmI, -1, null, item);
						Main.item[item].active = false;
					}
					else
						CommonCode.DropItem(info, itemId, 1);
				}
				result.State = ItemDropAttemptResultState.Success;
				return result;
			}

			result.State = ItemDropAttemptResultState.FailedRandomRoll;
			return result;
		}
	}

	public class MultiDropHelper<T>(int denominator = 1, int minQuantity = 1, int maxQuantity = 1, int numerator = 1) : MultiDropHelper(ModContent.ItemType<T>(), denominator, minQuantity, maxQuantity, numerator) where T : ModItem 
	{

	}

	public class AnyDropHelper(int[] itemIDs, int denominator = 1, int numerator = 1, int rolls = 1) : CommonDrop(itemIDs.FirstOrDefault(), denominator, chanceNumerator: numerator)
	{
		public int[] ids = itemIDs;
		public int rolls = rolls;

		public override ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			List<int> actualids = [..ids];
			ItemDropAttemptResult result = default;
			if (info.rng.Next(chanceDenominator) < chanceNumerator)
			{
				for (int i = 0; i < rolls; i++)
				{
					var id = Main.rand.Next(actualids);
					actualids.Remove(id);
					if (!(id <= 0 || id >= ItemLoader.ItemCount))
					{
						CommonCode.DropItem(info, id, Main.rand.Next(amountDroppedMinimum, amountDroppedMaximum + 1));
					}
				}
				result.State = ItemDropAttemptResultState.Success;
				return result;
			}

			result.State = ItemDropAttemptResultState.FailedRandomRoll;
			return result;
		}

		public override void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			float num = (float)chanceNumerator / (float)chanceDenominator;
			float dropRate = num * ratesInfo.parentDroprateChance;
			dropRate /= ids.Length / (float)rolls;
			foreach (var id in ids)
			{
				drops.Add(new DropRateInfo(id, amountDroppedMinimum, amountDroppedMaximum, dropRate, ratesInfo.conditions));
			}
			Chains.ReportDroprates(ChainedRules, num, drops, ratesInfo);
		}

		public static AnyDropHelper Create(params int[] ids)
		{
			return new AnyDropHelper(ids);
		}
	}

	public class MultiAnyDropHelper(int[][] itemIDs, int denominator = 1, int numerator = 1) : CommonDrop(itemIDs.FirstOrDefault().FirstOrDefault(), denominator, chanceNumerator: numerator)
	{
		public int[][] ids = itemIDs;

		public override ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			var id = Main.rand.Next(ids);
			ItemDropAttemptResult result = default;
			if (info.rng.Next(chanceDenominator) < chanceNumerator)
			{
				foreach (var realid in id)
				{
					if (!(realid <= 0 || realid >= ItemLoader.ItemCount))
					{
						CommonCode.DropItem(info, realid, 1);
					}
				}
				result.State = ItemDropAttemptResultState.Success;
				return result;
			}

			result.State = ItemDropAttemptResultState.FailedRandomRoll;
			return result;
		}

		public override void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			float num = (float)chanceNumerator / (float)chanceDenominator;
			float dropRate = num * ratesInfo.parentDroprateChance;
			dropRate /= (float)ids.Length;
			foreach (var id in ids)
			{
				foreach (var realid in id)
				{
					drops.Add(new DropRateInfo(realid, amountDroppedMinimum, amountDroppedMaximum, dropRate, ratesInfo.conditions));
				}
			}
			Chains.ReportDroprates(ChainedRules, num, drops, ratesInfo);
		}

		public static AnyDropHelper Create(params int[] ids)
		{
			return new AnyDropHelper(ids);
		}
	}

	public class Mastvengence : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info) => ExternalModSupport.Mastvengence;

		public bool CanShowItemDropInUI() => ExternalModSupport.Mastvengence;

		public string GetConditionDescription()
		{
			if (ExternalModSupport.HasCalamity && !Main.masterMode)
				return Language.GetTextValue("Mods.CalamityMod.Condition.InRev");

			return Language.GetTextValue("Bestiary_ItemDropConditions.IsMasterMode");
		}
	}

	public class QuickDropRule(Predicate<DropAttemptInfo> lambda, bool ui = true, string desc = null) : IItemDropRuleCondition
	{
		private readonly Predicate<DropAttemptInfo> condition = lambda;

		private readonly bool visibleInUI = ui;

		private readonly string description = desc;

		public bool CanDrop(DropAttemptInfo info) => condition(info);

		public bool CanShowItemDropInUI() => visibleInUI;

		public string GetConditionDescription() => description;
	}
}
