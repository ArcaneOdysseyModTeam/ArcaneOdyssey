using ArcaneOdyssey.Biomes;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Consumable;
using ArcaneOdyssey.NPCs.Bosses;
using ArcaneOdyssey.Projectiles;
using ArcaneOdysseyMusic.MusicBoxes;
using System;
using System.Collections.Generic;

namespace ArcaneOdyssey.AOPlayers
{
	public partial class AOPlayer : ModPlayer, IImbuable
	{
		public Imbuable Imbue { get; set; }
		public short StatSize = 0;
		public Circle myCircle = null;
		public ushort timeTillNextMove = 0;
		public List<Cooldown> Cooldowns = [];
		public bool HeavySkillActive = false;
		public bool hasLoadedWorldBefore = false;
		public bool Immobile => timeTillNextMove > 0 || (!CanMoveOnGround && HeavySkillActive);
		public bool CanMoveOnGround;
		public bool grounded = false;
		public bool FirstFrozenFrame => timeSinceSoftFrozen < 1;
		public ushort timeSinceSoftFrozen;
		public sbyte hasWings = 2;

		public override void OnEnterWorld()
		{
			hasWings = 2;
		}

		public static bool evil => !EliusSpareSystem.spared;


		public List<ImbueDebuffHelper> DebuffHelpers = [];

		public float MaxRunSpeed => Math.Max(Player.accRunSpeed, Player.maxRunSpeed);

		public float MaxPossibleSpeed => Math.Max(MaxRunSpeed, CurrentDash?.DashSpeed ?? MaxRunSpeed);

		public void UpdateDebuffHelpers(int damageDone, NPC target, Imbuable imbue = null, bool useplayerimbue = true, bool canAddBuffs = true)
		{
			if (useplayerimbue)
				imbue ??= Imbue;
			if (!(target.CountsAsACritter || target.friendly || Main.npcCatchable[target.type]))
			{
				if (imbue is not null)
				{
					foreach (Debuff buff in imbue.ImbueDebuffs)
					{
						var dur = buff.debuffDuration != 0 ? buff.debuffDuration : damageDone;
						var index = DebuffHelpers.FindIndex(e => e.buffID == buff.debuffID && e.imbue.Type == imbue.Type && e.npc.type == target.type);
						if (index != -1)
						{
							var instance = DebuffHelpers[index];
							var damage = instance.damagedone + damageDone;
							if (canAddBuffs && (((float)damage / target.lifeMax) >= buff.debuffPercent))
							{
								target.AddBuff(buff.debuffID, dur);
								damage = 0;
							}
							DebuffHelpers[index] = instance with { damagedone = damage };
						}
						else
						{
							if (canAddBuffs && (((float)damageDone / target.lifeMax) >= buff.debuffPercent))
							{
								target.AddBuff(buff.debuffID, dur);
							}
							else
							{
								DebuffHelpers.Add(new(imbue, damageDone, target, buff.debuffID));
							}
						}
					}
				}
			}
		}

		public override void Load()
		{
			On_Player.ApplyDamageToNPC += AoEHelper;
		}

		private static void AoEHelper(On_Player.orig_ApplyDamageToNPC orig, Player self, NPC npc, int damage, float knockback, int direction, bool crit, DamageClass damageType, bool damageVariation)
		{
			Imbuable imbue = null;
			imbue ??= AOUtils.Safe<Imbuable>(self.PlayerItem()?.ModItem);
			imbue ??= self.PlayerItem().Imbue();
			imbue ??= self.Imbue();
			imbue?.Gimmick?.OnHitNPC(imbue, self, npc, npc.CalculateHitInfo(damage, direction, crit, knockback, damageType, damageVariation), damage);
			if (imbue is SpiritEnergy)
				if (!npc.immortal)
					self.ArcaneOdyssey()?.TrySpiritLifesteal(damage);
			self.ArcaneOdyssey()?.UpdateDebuffHelpers(damage, npc, imbue, false);
			orig(self, npc, damage, knockback, direction, crit, damageType, damageVariation);
		}

		public override void Unload()
		{
			On_Player.ApplyDamageToNPC -= AoEHelper;
		}

		public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
		{
			UpdateDebuffHelpers(damageDone, target, item.Imbue(), false, true);
			UpdateDebuffHelpers(damageDone, target, item.SecondImbue(), false, true);
		}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
		{
			UpdateDebuffHelpers(damageDone, target, proj.Imbue(), false, true);
			UpdateDebuffHelpers(damageDone, target, proj.SecondImbue(), false, true);
		}

		public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
		{
			List<Item> items = [];
			if (!mediumCoreDeath)
			{
				items.Add(new Item(ModContent.ItemType<EagleLegacy>()));
				items.Add(new Item(ModContent.ItemType<TitleMusicBox>()));
			}
			else
			{
				foreach (var imbue in Player.inventory)
				{
					if (imbue.ModItem is Imbuable)
					{
						items.Add(new Item(imbue.type));
					}
				}
			}
			return items;
		}

		public void TrySpiritLifesteal(int damage, bool cooldown = true)
		{
			if (!(cooldown && OnCooldown("SpiritLifesteal")))
			{
				if (cooldown)
					SetCooldown(new Cooldown("SpiritLifesteal", Mod, 60 * 2));
				Player.Heal(Utils.Clamp(damage / 5, 1, 15 + AOUtils.BossesKilled));
			}
		}

		public override void PostUpdate()
		{
			if (!hasLoadedWorldBefore)
			{
				hasLoadedWorldBefore = true;
				if (!ModLoader.HasMod("NMMSI"))
				{
					if (Main.myPlayer == Player.whoAmI)
					{
						if (!Player.HasTypeInInventory<EagleLegacy>())
						{
							Player.QuickSpawnItem(Player.GetSource_FromThis(), ModContent.ItemType<EagleLegacy>());
						}
						if (!Player.HasTypeInInventory<TitleMusicBox>())
						{
							Player.QuickSpawnItem(Player.GetSource_FromThis(), ModContent.ItemType<TitleMusicBox>());
						}
					}
				}
			}
			pheonixHealing = 0;
			ArcaneOdysseyMod.Sets.phoenixAffected = NPCID.Sets.Factory.CreateBoolSet();
			HeavySkillActive = false;

			if (Imbue is not null && !Imbue.PlayerHasImbue(Player))
			{
				Imbue = null;
			}

			Player.statDefense -= _defenseLost;

			if (Player.InModBiome<EliusArena>())
			{
				Player.AddBuff(BuffID.NoBuilding, 2); // entirely visual
				if (AOUtils.ServerOrSingleplayer)
				{
					if (NPC.downedBoss1)
					{
						if (!AOUtils.BossAlive)
						{
							if (eliusArenaCounter <= (30 * 60))
								eliusArenaCounter++;
						}
						else
						{
							eliusArenaCounter = 0;
						}

						if (eliusArenaCounter >= (30 * 60)) // 30 seconds
						{
							if (Main.raining || !DownedBosses.DownedElius)
							{
								//elius spawn location
								NPC.SpawnBoss((EliusArenaLoader.eliusArena.Center.X + 25) * 16, (EliusArenaLoader.eliusArena.Center.Y + 2) * 16, ModContent.NPCType<LordElius>(), Player.whoAmI);
							}
						}
					}
				}
			}
			else
			{
				eliusArenaCounter = 0;
				if (AOUtils.NPCAlive<LordElius>())
					Player.AddBuff(BuffID.Electrified, 2);
			}
		}

		public float SpaceGravityMulti
		{
			get
			{
				float x = Main.maxTilesX / 4200f;
				x *= x;
				return (float)((Player.position.Y / 16f - (60f + 10f * x)) / (Main.worldSurface / (Main.remixWorld ? 1.0 : 6.0)));
			}
		}

		public bool InSpace => SpaceGravityMulti < 1f;

		public void FreezeMovement()
		{
			if (Math.Abs(Player.velocity.Y) < .5f && Player.wingTime == Player.wingTimeMax && Player.wingFrame == 0 && !Player.controlJump && !Player.TryingToHoverDown && !Player.TryingToHoverUp)
			{
				grounded = true;
			}
			else
			{
				grounded = false;
			}
			if (HeavySkillActive)
			{
				if (FirstFrozenFrame)
				{
					CanMoveOnGround = grounded;
				}
				if (!CanMoveOnGround)
				{
					Player.gravity = 0f;
					Player.velocity.X *= .001f;
					Player.velocity.Y *= .001f;
				}
				timeSinceSoftFrozen++;
			}
			else
			{
				timeSinceSoftFrozen = 0;
				CanMoveOnGround = false;
			}
		}

		public override void ResetEffects()
		{
			if (Player.InModBiome<EliusArena>())
			{
				Player.noBuilding = true;
			}
			if (ZapCD > 0)
			{
				ZapCD--;
			}
			else
			{
				ZapCD = 0;
			}
			StatSize = 0;
			StatHaste = 0;
			Insanity = 0;
			Banishment = 0;
			if (hasWings > 0)
				hasWings--;
			ResetBuffs();
			HandleDashDetection();
		}

		public float SizeMulti => 1f + (StatSize / (BaseArmour.SizeDivision * 100f));
		public float CooldownDurationMulti => Math.Max(1f - (StatHaste / (BaseArmour.HasteDivision * 100f)), .25f);
	}
}
