using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.Items.Armour.Vanity;
using ArcaneOdyssey.Items.Blocks;
using ArcaneOdyssey.Items.Consumable;
using ArcaneOdyssey.Items.Materials;
using ArcaneOdyssey.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.GlobalTypes
{
	/// <summary>
	/// basically AOPlayer but for npcs
	/// </summary>
	public class AONPC : GlobalNPC
	{
		public override bool InstancePerEntity => true;

		public int StunCD = 5 * 60;
		public int ZapCD = 5 * 50; // ancient lightning chain
		public int StunDuration = 60;

		private int _defenseLost = 0;

		public void LowerDefense(int defense, Rectangle? location = null)
		{
			_defenseLost += defense;
			if (location.HasValue)
				CombatText.NewText(location.Value, Color.Gray, -defense, true);
		}

		#region Debuff bools
		public bool bleeding = false;
		public bool scalding = false;
		public bool burning = false;
		public bool scorched = false;
		public bool poisoned = false;
		public bool shadowflame = false;
		public bool melting = false;
		public bool corroding = false;
		public bool vesuvianBurn = false;
		public bool seared = false;
		public int singedstacks = 0;
		public bool elecToxins = false;
		public bool phoenixDrain = false;
		public int lesserPhoenixDrain = 0;
		public bool ionized = false;

		public bool ashcursed = false;

		public bool AOStunned = false;
		#endregion

		public override bool PreAI(NPC npc)
		{
			if (Main.dedServ || Main.netMode == NetmodeID.SinglePlayer)
			{
				if (!AOStunned)
				{
					if (StunCD > 0)
					{
						StunCD--;
					}
					else
					{
						StunCD = 0;
					}
				}
				else
				{
					if (StunDuration > 0)
					{
						StunDuration--;
					}
					else
					{
						StunDuration = 0;
					}
				}
			}
			return !AOStunned;
		}

		public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
		{
			modifiers.ArmorPenetration += _defenseLost;
			if (ashcursed)
			{
				modifiers.ScalingArmorPenetration += .1f;
			}
		}

		public override void ResetEffects(NPC npc)
		{
			if (ZapCD > 0)
			{
				ZapCD--;
			}
			else
			{
				ZapCD = 0;
			}
			if (StunDuration <= 0 && AOStunned)
			{
				AOStunned = false;
				StunCD = 5 * 60;
				StunDuration = 60;
			}
			bleeding = false;
			vesuvianBurn = false;
			scalding = false;
			singedstacks = 0;
			seared = false;
			elecToxins = false;
			poisoned = false;
			phoenixDrain = false;
			melting = false;
			shadowflame = false;
			burning = false;
			scorched = false;
			corroding = false;
			ionized = false;
			ashcursed = false;
			lesserPhoenixDrain = 0;
		}

		public override void UpdateLifeRegen(NPC npc, ref int damage)
		{
			void Apply(float percentPerSecond, ref int damage, int? min = null, int? max = null)
			{
				var damagepercentage = percentPerSecond / 50f;
				if (npc.boss)
				{
					damagepercentage /= 6;
				}
				var loss = Utils.Clamp((int)(npc.lifeMax * damagepercentage), min.GetValueOrDefault(percentPerSecond.Round()), max.GetValueOrDefault((1500 * percentPerSecond).Round()));
				npc.lifeRegen -= loss;
				if (damage < 0)
					damage = loss / 4;
				else
					damage += loss / 4;
			}
			if (bleeding)
			{
				Apply(.8f, ref damage);
				if (ionized)
				{
					Apply(.5f, ref damage);
				}
			}
			if (poisoned)
			{
				Apply(1.2f, ref damage);
			}
			if (burning)
			{
				Apply(1f, ref damage);
				if (npc.oiled)
				{
					Apply(.25f, ref damage);
				}
			}
			if (scalding)
			{
				Apply(1.5f, ref damage);
				if (ionized)
				{
					Apply(.5f, ref damage);
				}
				// since its steam being oiled up does nothing
			}
			if (corroding) // same dot as melting!
			{
				Apply(1.8f, ref damage);
			}
			if (melting)
			{
				Apply(1.8f, ref damage);
				if (npc.oiled)
				{
					Apply(.25f, ref damage);
				}
				if (ionized)
				{
					Apply(.5f, ref damage);
				}
			}
			if (shadowflame)
			{
				Apply(2f, ref damage);
				if (npc.oiled)
				{
					Apply(.25f, ref damage);
				}
				if (ionized)
				{
					Apply(.5f, ref damage);
				}
			}
			if (scorched)
			{
				Apply(1.6f, ref damage);
				if (npc.oiled)
				{
					Apply(.25f, ref damage);
				}
				if (ionized)
				{
					Apply(.5f, ref damage);
				}
			}
			if (seared)
			{
				Apply(1.4f, ref damage);
				if (npc.oiled)
				{
					Apply(.25f, ref damage);
				}
				if (ionized)
				{
					Apply(.5f, ref damage);
				}
			}
			if (singedstacks > 0)
			{
				Apply(.75f * singedstacks, ref damage);
				if (npc.oiled)
				{
					Apply(.25f, ref damage);
				}
				if (ionized)
				{
					Apply(.5f, ref damage);
				}
			}
			if (elecToxins)
			{
				Apply(2.2f, ref damage);
				if (ionized)
				{
					Apply(.5f, ref damage);
				}
			}
			if (ionized)
			{
				Apply(2.5f, ref damage);
			}
			if (phoenixDrain)
			{
				if (lesserPhoenixDrain > 0)
					Apply(.25f * lesserPhoenixDrain, ref damage);
				else
					Apply(1.3f, ref damage);
				if (npc.oiled)
				{
					Apply(.25f, ref damage);
				}
				if (ionized)
				{
					Apply(.5f, ref damage);
				}
			}
			if (vesuvianBurn)
			{
				Apply(8f, ref damage, 10, 10000);
				if (npc.oiled)
				{
					Apply(.25f, ref damage);
				}
				if (ionized)
				{
					Apply(.5f, ref damage);
				}
			}
		}
	}

	public class NPCLootManager : GlobalNPC
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
				LeadingConditionRule leadingConditionRule = new(new SpiritMechDropCondition());
				leadingConditionRule.OnSuccess(new MechBossSpiritDropper());
				npcLoot.Add(leadingConditionRule);
			}
		}

		public override void ModifyGlobalLoot(GlobalLoot globalLoot)
		{
			LeadingConditionRule AcrimonyCondition = new(new NoShowNoConditon());
			AcrimonyCondition.OnSuccess(AOUtils.Common<Acrimony>(3000));
			globalLoot.Add(AcrimonyCondition);
		}

		public override void OnKill(NPC npc)
		{
			if (npc.type == NPCID.HallowBoss)
			{
				if (npc.AI_120_HallowBoss_IsGenuinelyEnraged())
				{
					DownedBosses.downedEnragedEmpress = true;
					if (Main.dedServ)
					{
						NetMessage.SendData(MessageID.WorldData);
					}
				}
			}

			if ((npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsTail || npc.type == NPCID.EaterofWorldsBody) && !AOUtils.EoWStillAlive)
			{
				if (!DownedBosses.downedWorldEater)
				{
					DownedBosses.downedWorldEater = true;
					if (Main.dedServ)
					{
						NetMessage.SendData(MessageID.WorldData);
					}
				}
			}

			if (npc.type == NPCID.BrainofCthulhu)
			{
				DownedBosses.downedBrain = true;
				if (Main.dedServ)
				{
					NetMessage.SendData(MessageID.WorldData);
				}
			}

			if (npc.type == NPCID.EyeofCthulhu)
			{
				if (!NPC.downedBoss1)
				{
					if (Main.dedServ)
					{
						ChatHelper.BroadcastChatMessage(Mod.CustomLocalization("MenacingMessages.EliusAvailable").ToNetworkText(), Color.MediumPurple);
					}
					else
					{
						Main.NewText(Mod.CustomLocalization("MenacingMessages.EliusAvailable").Value, Color.MediumPurple);
					}
				}
			}

			if (npc.type == NPCID.TownSlimePurple)
				Projectile.NewProjectile(npc.GetSource_Death(), npc.Center, new(0, 10), ModContent.ProjectileType<DeathCurse>(), 700, 0f);
		}

		public override void ModifyShop(NPCShop shop)
		{
			if (shop.NpcType == NPCID.Clothier)
			{
				shop.Add<WhiteEyesPlush>();
			}
		}
	}
}
