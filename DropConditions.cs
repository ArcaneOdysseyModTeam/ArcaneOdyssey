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
		public bool CanDrop(DropAttemptInfo info) => !DownedBosses.downedEvander;
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => Language.GetOrRegister($"Mods.{ArcaneOdysseyMod.InternalName}.DropConditions.FirstEvanderKillDescription", () => "First Evander Defeated").Value;
	}
	public class NotFirstEvanderKill : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info) => DownedBosses.downedEvander;
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => Language.GetOrRegister($"Mods.{ArcaneOdysseyMod.InternalName}.DropConditions.NotFirstEvanderKillDescription", () => "Following Evanders Defeated").Value;
	}

	public class FirstDayEmpressKill : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info) => !DownedBosses.downedEnragedEmpress;
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
				return (!NPC.downedMechBoss2) && !AOUtils.BothTwinsAlive();

			if (NPC.downedMechBoss3 && NPC.downedMechBoss2)
				return !NPC.downedMechBoss1;

			return false;
		}

		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => Language.GetOrRegister($"Mods.{ArcaneOdysseyMod.InternalName}.DropConditions.FirstMechBossesKillDescription", () => "First Mechanical Trio Defeated").Value;
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
}
