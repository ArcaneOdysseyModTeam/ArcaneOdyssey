using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.NPCS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

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

		public bool Bleeding = false;

		public bool AOStunned = false;

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
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
			if (Bleeding)
			{
				npc.lifeRegen -= 3;
			}
        }
    }

	public class AOGlobalNPC : GlobalNPC
	{
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
			if (npc.type == NPCID.WallofFlesh)
			{
				LeadingConditionRule leadingConditionRule = new(new Conditions.IsPreHardmode());
				leadingConditionRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<PoseidonChoice>()));
				npcLoot.Add(leadingConditionRule);
			}
			if (npc.type == NPCID.CultistBoss)
			{
				LeadingConditionRule leadingConditionRule = new(new FirstCultistKill());
				leadingConditionRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<PoseidonChoice>()));
				npcLoot.Add(leadingConditionRule);
			}
			if (npc.type == NPCID.Plantera)
			{
				LeadingConditionRule leadingConditionRule = new(new Conditions.FirstTimeKillingPlantera());
				leadingConditionRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<HecateShard>()));
				npcLoot.Add(leadingConditionRule);
			}
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Acrimony>(), 6000));
		}
	}
}
