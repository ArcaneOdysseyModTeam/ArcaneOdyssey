using ArcaneOdyssey.Content.Items.Armour.Vanity;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Lost;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.NPCS;
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
		public float ZapCD = 5; // ancient lightning chain
		public float StunDuration = 1;

		#region Debuff bools
		public bool Bleeding = false;
		public bool HeavyBleeding = false;
		public bool Scalding = false;
		public bool Seared = false;
		public bool ElecToxins = false;

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

		public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
		{
			player.ArcaneOdyssey().UpdateDebuffHelpers(damageDone, npc, item.Imbue());
		}

		public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
		{
			if (projectile.TryGetOwner(out var player))
				player.ArcaneOdyssey().UpdateDebuffHelpers(damageDone, npc, projectile.Imbue());
		}

		public override void ResetEffects(NPC npc)
		{
			ZapCD -= 1 / 60f;
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
			ElecToxins = false;
		}

		public override void UpdateLifeRegen(NPC npc, ref int damage)
		{
			// onfire is 4 * 2, or 8
			// poison is 6 * 2, or 12
			// frostburn is 8 * 2, or 16
			// shadowflame is 15 * 2, or 30
			// cursed inferno is 24 * 2, or 48
			// acid venom is 30 * 2, or 60
			if (npc.ModNPC is not Edgelord) // morden is immune to dot lol
			{
				if (Bleeding)
				{
					npc.lifeRegen -= 10;
				}
				if (HeavyBleeding)
				{
					npc.lifeRegen -= 20;
				}
				if (Scalding)
				{
					npc.lifeRegen -= 25;
				}
				if (Seared)
				{
					npc.lifeRegen -= 15;
				}
				if (ElecToxins)
				{
					npc.lifeRegen -= 80;
				}
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
				leadingConditionRule.OnSuccess(new HecateDropMultiHelper(ModContent.ItemType<PoseidonSpirit>()));
				npcLoot.Add(leadingConditionRule);
			}
			if (npc.type == NPCID.CultistBoss)
			{
				LeadingConditionRule leadingConditionRule = new(new FirstCultistKill());
				leadingConditionRule.OnSuccess(new HecateDropMultiHelper(ModContent.ItemType<PoseidonSpirit>()));
				npcLoot.Add(leadingConditionRule);
			}
			if (npc.type == NPCID.HallowBoss)
			{
				LeadingConditionRule leadingConditionRule = new(new FirstEmpressKill());
				leadingConditionRule.OnSuccess(new HecateDropMultiHelper(ModContent.ItemType<HecateShard>()));
				npcLoot.Add(leadingConditionRule);
				//LeadingConditionRule leadingConditionRule1 = new(new FirstDayEmpressKill());
				//leadingConditionRule1.OnSuccess(new HecateDropMultiHelper(ModContent.ItemType<PoseidonSpirit>()));
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
				leadingConditionRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<HeadlessHead>(), 30));
				npcLoot.Add(leadingConditionRule);
			}
			LeadingConditionRule AcrimonyCondition = new(new NoShowNoConditon());
			AcrimonyCondition.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Acrimony>(), 6000));
			npcLoot.Add(AcrimonyCondition);
		}
	}
}
