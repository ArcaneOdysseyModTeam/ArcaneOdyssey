using System;
using ArcaneOdyssey.Content.Items;
using ArcaneOdyssey.Content.Items.Armour.Vanity;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Lost;
using ArcaneOdyssey.Content.Items.Materials;
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
		public bool bleeding = false;
		public bool heavyBleeding = false;
		public bool scalding = false;
		public bool vesuvianBurn = false;
		public bool seared = false;
		public int singedstacks = 0;
		public bool elecToxins = false;
		public bool phoenixDrain = false;
		public int lesserPhoenixDrain = 0;

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
			player.ArcaneOdyssey().UpdateDebuffHelpers(damageDone, npc, item.Imbue(), false);
		}

		public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
		{
			if (projectile.TryGetOwner(out var player))
				player.ArcaneOdyssey().UpdateDebuffHelpers(damageDone, npc, projectile.Imbue(), false);
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
			bleeding = false;
			vesuvianBurn = false;
			heavyBleeding = false;
			scalding = false;
			singedstacks = 0;
			seared = false;
			elecToxins = false;
			phoenixDrain = false;
			lesserPhoenixDrain = 0;
		}

		public override void UpdateLifeRegen(NPC npc, ref int damage)
		{
			// onfire is 4 * 2, or 8
			// poison is 6 * 2, or 12
			// frostburn is 8 * 2, or 16
			// shadowflame is 15 * 2, or 30
			// cursed inferno is 24 * 2, or 48
			// acid venom is 30 * 2, or 60
			if (bleeding)
			{
				npc.lifeRegen -= 10;
			}
			if (heavyBleeding)
			{
				npc.lifeRegen -= 20;
			}
			if (scalding)
			{
				npc.lifeRegen -= 25;
			}
			if (seared)
			{
				npc.lifeRegen -= 15;
			}
			if (singedstacks > 0)
			{
				npc.lifeRegen -= 6 * singedstacks;
			}
			if (elecToxins)
			{
				npc.lifeRegen -= 80;
			}
			if (phoenixDrain)
			{
				if (lesserPhoenixDrain > 0)
					npc.lifeRegen -= 5 * lesserPhoenixDrain;
				else
					npc.lifeRegen -= 14;
			}
			if (vesuvianBurn)
			{
				npc.GetLifeStats(out int npcCurrentlife, out _);
				npc.lifeRegen -= Utils.Clamp((int)MathF.Ceiling(npcCurrentlife * 0.4f), 10, 10000);
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
				leadingConditionRule.OnSuccess(new MultiDropHelper<PoseidonSpirit>());
				npcLoot.Add(leadingConditionRule);
			}
			if (npc.type == NPCID.Golem)
			{
				LeadingConditionRule leadingConditionRule = new(new FirstGolemKill());
				leadingConditionRule.OnSuccess(new MultiDropHelper<HecateShard>());
				npcLoot.Add(leadingConditionRule);
			}
			if (npc.type == NPCID.HallowBoss)
			{
				LeadingConditionRule leadingConditionRule = new(new FirstDayEmpressKill());
				leadingConditionRule.OnSuccess(new MultiDropHelper<HecateShard>());
				npcLoot.Add(leadingConditionRule);
			}
			if (npc.type == NPCID.MoonLordCore)
			{
				LeadingConditionRule leadingConditionRule = new(new FirstMoonLordKill());
				leadingConditionRule.OnSuccess(new MultiDropHelper<AncientHecateOrb>());
				npcLoot.Add(leadingConditionRule);
			}
			if (npc.type == NPCID.HeadlessHorseman)
			{
				LeadingConditionRule leadingConditionRule = new(new NoShowNoConditon());
				leadingConditionRule.OnSuccess(AOUtils.Common<HeadlessHead>(30));
				npcLoot.Add(leadingConditionRule);
			}
			if (npc.type == NPCID.SkeletronPrime || npc.type == NPCID.TheDestroyer || npc.type == NPCID.Retinazer || npc.type == NPCID.Spazmatism)
			{
				LeadingConditionRule leadingConditionRule = new(new DownedAllMechBossesFirstTime());
				leadingConditionRule.OnSuccess(new MultiDropHelper<PoseidonSpirit>());
				npcLoot.Add(leadingConditionRule);
			}
			LeadingConditionRule AcrimonyCondition = new(new NoShowNoConditon());
			AcrimonyCondition.OnSuccess(AOUtils.Common<Acrimony>(3000));
			npcLoot.Add(AcrimonyCondition);
		}
	}
}
