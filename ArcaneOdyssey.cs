using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Equipment.MusicBoxes;
using ArcaneOdyssey.Content.Items.Materials;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArcaneOdyssey
{
	public class ArcaneOdyssey : Mod 
	{
		public static Dictionary<string, LocalizedText> staticLocalizer = [];
	}

	public class NPCDrops : GlobalNPC
	{
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
			if (npc.type == NPCID.WallofFlesh)
			{
				LeadingConditionRule leadingConditionRule = new LeadingConditionRule(new Conditions.IsPreHardmode());
				leadingConditionRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<HecateOrb>()));
				npcLoot.Add(leadingConditionRule);
			}
			if (npc.type == NPCID.CultistBoss)
			{
				LeadingConditionRule leadingConditionRule = new LeadingConditionRule(new FirstCultistKill());
				leadingConditionRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<HecateOrb>()));
				npcLoot.Add(leadingConditionRule);
			}
			if (npc.type == NPCID.Plantera)
			{
				LeadingConditionRule leadingConditionRule = new LeadingConditionRule(new Conditions.FirstTimeKillingPlantera());
				leadingConditionRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<HecateShard>()));
				npcLoot.Add(leadingConditionRule);
			}
		}
	}

	public class FirstCultistKill : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info) => !NPC.downedAncientCultist;
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => Language.GetOrRegister("Mods.ArcaneOdyssey.FirstCultistKillDescription", () => "First Lunatic Cultist Defeated").Value;
	}


	public class AOPlayer : ModPlayer
	{
		public AOMagic imbue = null;

		/// <summary>
		/// Whether the user has a set of sunken armour equipped
		/// </summary>
		public bool sunkenArmour = false;

		public int AOSizeStat = 0;

		public bool RightClicking => Player.altFunctionUse == 2;

		public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
		{
			if (!mediumCoreDeath)
			{
				return [new Item(ModContent.ItemType<HecateOrb>()), new Item(ModContent.ItemType<TitleMusicBox>())];
			}
			else return [];
		}

		public override void ResetEffects()
		{
			sunkenArmour = false;
			AOSizeStat = 0;
		}

		public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
		{
			if (sunkenArmour)
			{
				npc.AddBuff(BuffID.Wet, 60 * 10);
			}
		}

		public float GetSizeMulti(Item item)
		{
			float stat = AOSizeStat / 300f;
			if (Player.meleeScaleGlove && item.DamageType.Name.Contains("Melee"))
			{
				stat += .1f;
			}
			return stat+1;
		}

		public float GetSizeMulti(Projectile projectile)
		{
			float stat = AOSizeStat / 300f;
			if (Player.meleeScaleGlove && projectile.DamageType.Name.Contains("Melee"))
			{
				stat += .1f;
			}
			return stat + 1f;
		}
	}
}
