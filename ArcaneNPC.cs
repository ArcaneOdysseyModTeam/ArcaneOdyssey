using ArcaneOdyssey.Content.Items.Imbues.Magic.Lost;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Items.Vanity;
using ArcaneOdyssey.Content.NPCS;
using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey
{
	/// <summary>
	/// basically AOPlayer but for npcs
	/// </summary>
	public class ArcaneNPC : GlobalNPC
	{
		public override bool InstancePerEntity => true;

		public float StunCD = 5;
		public float StunDuration = 1;

		#region Debuff bools
		public bool Bleeding = false;
		public bool HeavyBleeding = false;
		public bool Scalding = false;
		public bool Seared = false;

		public bool AOStunned = false;
		#endregion

		public override bool PreAI(NPC npc)
		{
			if (Main.dedServ || Main.netMode == NetmodeID.SinglePlayer)
			{
				if (!AOStunned)
					StunCD -= 1 / 60;
				else
					StunDuration -= 1 / 60;
			}
			return !AOStunned;
		}

		public override void ResetEffects(NPC npc)
		{
			if (StunDuration <= 0 && AOStunned)
			{
				AOStunned = false;
				StunCD = 5;
				StunDuration = 1;
			}
			Bleeding = false;
			HeavyBleeding = false;
			Scalding = false;
			Seared = false;
		}

		public override void UpdateLifeRegen(NPC npc, ref int damage)
		{
			if (Bleeding)
			{
				npc.lifeRegen -= 3;
			}
			if (HeavyBleeding)
			{
				npc.lifeRegen -= 6;
			}
			if (Scalding)
			{
				npc.lifeRegen -= 5;
			}
			if (Seared) {
				npc.lifeRegen -= 4;
			}
		}

	}

	public class AOGlobalNPC : GlobalNPC
	{
		public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
		{
			if (item.Imbue() is GravityMagic)
				modifiers.HitDirectionOverride = modifiers.HitDirection * -1;
		}

		public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
		{
			if (projectile.Imbue() is GravityMagic)
				modifiers.HitDirectionOverride = modifiers.HitDirection * -1;
		}

		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
			if (npc.type == NPCID.WallofFlesh)
			{
				LeadingConditionRule leadingConditionRule = new(new Conditions.IsPreHardmode());
				leadingConditionRule.OnSuccess(new HecateDropMultiHelper(ModContent.ItemType<PoseidonChoice>()));
				npcLoot.Add(leadingConditionRule);
			}
			if (npc.type == NPCID.CultistBoss)
			{
				LeadingConditionRule leadingConditionRule = new(new FirstCultistKill());
				leadingConditionRule.OnSuccess(new HecateDropMultiHelper(ModContent.ItemType<PoseidonChoice>()));
				npcLoot.Add(leadingConditionRule);
			}
			if (npc.type == NPCID.HallowBoss)
			{
				LeadingConditionRule leadingConditionRule = new(new FirstEmpressKill());
				leadingConditionRule.OnSuccess(new HecateDropMultiHelper(ModContent.ItemType<HecateShard>()));
				npcLoot.Add(leadingConditionRule);
				//LeadingConditionRule leadingConditionRule1 = new(new FirstDayEmpressKill());
				//leadingConditionRule1.OnSuccess(new HecateDropMultiHelper(ModContent.ItemType<PoseidonChoice>()));
				//npcLoot.Add(leadingConditionRule1);
			}
			if (npc.type == NPCID.Plantera)
			{
				LeadingConditionRule leadingConditionRule = new(new Conditions.FirstTimeKillingPlantera());
				leadingConditionRule.OnSuccess(new HecateDropMultiHelper(ModContent.ItemType<HecateShard>()));
				npcLoot.Add(leadingConditionRule);
			}
			if (npc.type == NPCID.MoonLordCore)
			{
				LeadingConditionRule leadingConditionRule = new(new FirstMoonLordKill());
				leadingConditionRule.OnSuccess(new HecateDropMultiHelper(ModContent.ItemType<AncientHecateOrb>()));
				npcLoot.Add(leadingConditionRule);
			}
			if (npc.type == NPCID.HeadlessHorseman)
			{
				LeadingConditionRule leadingConditionRule = new(new NoShowNoConditon());
				leadingConditionRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<HeadlessHead>(), 100));
				npcLoot.Add(leadingConditionRule);
			}
			LeadingConditionRule AcrimonyCondition = new(new NoShowNoConditon());
			AcrimonyCondition.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Acrimony>(), 6000));
			npcLoot.Add(AcrimonyCondition);
		}

		public override void OnKill(NPC npc)
		{
			if (npc.type == NPCID.HallowBoss)
			{
				if (npc.AI_120_HallowBoss_IsGenuinelyEnraged())
				{
					DownedBosses.downedEnragedEmpress = true;
					if (Main.dedServ)
						NetMessage.SendData(MessageID.WorldData);
				}
			}    
		}
	}
}
