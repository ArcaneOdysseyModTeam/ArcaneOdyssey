using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Projectiles;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic;
using ArcaneOdyssey.VFX.Rarities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Terraria;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArcaneOdyssey
{
	public static class AOUtils
	{
		public static Vector2 GetDrawOriginCentre(this Entity entity) => new(entity.width / 2, entity.height / 2);

		public static Imbuable Imbue(this Player player) => player.ArcaneOdyssey().Imbue;
		public static Imbuable Imbue(this ModPlayer player) => player.ArcaneOdyssey().Imbue;
		public static Imbuable Imbue(this Projectile projectile) => projectile.ArcaneOdyssey().Imbue;
		public static Imbuable Imbue(this ModProjectile projectile) => projectile.ArcaneOdyssey().Imbue;
		public static Imbuable Imbue(this Item item) => item.ArcaneOdyssey().Imbue;
		public static Imbuable Imbue(this ModItem item) => item.ArcaneOdyssey().Imbue;

		public static int Round(this float num) => (int)Math.Round(num);

		public static void Kill(this Entity entity)
		{
			if (entity is Projectile projectile)
			{
				projectile.Kill();
			}
		}

		public static bool BossAlive()
		{
			foreach (var npc in Main.ActiveNPCs)
			{
				if (npc.boss)
					return true;
			}
			return false;
		}

		public static DamageClass TrueMelee()
		{
			if (ModLoader.TryGetMod("CalamityMod", out Mod calamity))
			{
				return calamity.Find<DamageClass>("TrueMeleeDamageClass");
			}
			return DamageClass.Melee;
		}

		public static DamageClass TrueMeleeNoSpeed()
		{
			if (ModLoader.TryGetMod("CalamityMod", out Mod calamity))
			{
				return calamity.Find<DamageClass>("TrueMeleeNoSpeedDamageClass");
			}
			return DamageClass.MeleeNoSpeed;
		}

		public static float Clamp(this float num, float min, float max) => MathHelper.Clamp(num, min, max);

		public static List<Imbuable> GetAllImbues(this Player owner)
		{
			List<Imbuable> imbues = [];
			foreach (Item item in owner.inventory)
			{
				if (item.ModItem is Imbuable imbuable && item.ModItem is not FightingStyleBarred)
				{
					imbues.Add(imbuable);
				}
			}
			return imbues;
		}

		public static void SimulateAOE(float range, float damage, Vector2 origin, float knockback, Entity source, DamageClass damageClass, bool modifyimbuestats = true)
		{
			if (source is null) return;
			Imbuable imbue = source.AnyArcaneOdyssey()?.Imbue;
			if (imbue is not null)
			{
				if (modifyimbuestats)
				{
					if (source is Projectile proj && proj.ModProjectile is MagicSpell)
					{
						range *= imbue.AOScrollSize;
						knockback *= imbue.AOScrollSize;
						damage *= imbue.AOScrollDamage;
					}
					else
					{
						range *= imbue.AOImbueSize;
						knockback *= imbue.AOImbueSize;
						damage *= imbue.AOImbueDamage;
					}
				}


			}

			foreach (NPC target in Main.ActiveNPCs)
			{
				if (target.Hitbox.Distance(origin) <= range)
				{
					var modifiers = new DashDamageHelper();
					if (imbue is not null)
					{
						if (imbue is CrystalMagic && target.HasBuff<Crystallized>() && GetAOBuffStack(target, target.FindBuffIndex(ModContent.BuffType<Crystallized>())) == 4)
						{
							modifiers.FinalDamage += .3f;
						}

						foreach (var debuff in imbue.ImbueDebuffs)
						{
							if ((debuff.debuffPercent == 0) || modifiers.GetDamage(damage.Round()) > (target.lifeMax / debuff.debuffPercent))
							{
								target.AddBuff(debuff.debuffID, debuff.debuffDuration);
							}
						}

						if (imbue.CombinedDebuffs is not null)
						{
							foreach (CombinedDebuff buffkeys in imbue.CombinedDebuffs)
							{
								if (target.HasBuff(buffkeys.requirement) || (buffkeys.requirement == BuffID.Wet && target.wet))
								{
									target.AddBuff(buffkeys.result, buffkeys.duration);
								}
							}
						}

						foreach (MagicBuffMultiplier multiplier in imbue.Effects.magicBuffMultipliers)
						{
							if (target.HasBuff(multiplier.buffID) || (multiplier.buffID == BuffID.Wet && target.wet))
							{
								modifiers.FinalDamage += multiplier.multiplier.MultiToPercent();
							}
						}

						if (Main.netMode == NetmodeID.SinglePlayer) // things would get chaotic in multiplayer if everyone kept clearing eachothers debuffs
						{
							foreach (int buffid in imbue.Effects.clearBuffs)
							{
								if (target.HasBuff(buffid))
								{
									target.DelBuff(target.FindBuffIndex(buffid));
								}
							}
						}
					}
					if (modifiers.GetDamage(damage.Round()) > 0 && source.TryGetOwner(out Player player) && Main.myPlayer == player.whoAmI && !target.friendly && target.immune[player.whoAmI] <= 0)
					{ 
						target.SimpleStrikeNPC(modifiers.GetDamage(damage.Round()), ((target.Center - origin).X > 0).ToDirectionInt(), false, knockback, damageClass, true);
						target.immune[player.whoAmI] = 2;
					}
				}
			}
		}

		public static bool ImbueClassCheck(Projectile projectile)
		{
			if (projectile.active && (projectile.ModProjectile is null or AOPlayerProjectile || ArcaneOdysseyConfig.Instance.AffectsOtherMods) && (!global::ArcaneOdyssey.ArcaneOdyssey.excludedProjectiles.Contains(projectile.type)))
			{
				List<string> goodclasses = new(["TrueMeleeDamageClass", "TrueMeleeNoSpeedDamageClass", "MeleeRangedHybridDamageClass"]);
				if (goodclasses.Contains(projectile.DamageType.Name))
				{
					return true;
				}
				return (projectile.DamageType == DamageClass.Melee || projectile.DamageType == DamageClass.Ranged || projectile.ModProjectile is MagicSpell or StrengthTechnique or MagicCircle1 or MagicCircle2 or ExplosionTracker || projectile.DamageType == DamageClass.MeleeNoSpeed) && projectile.ModProjectile is not MagicCircle1 or MagicCircle2
					&& projectile.owner != 255 && !projectile.hostile && !projectile.npcProj && projectile.type != ProjectileID.FallingStar;
			}
			return false;
		}

		public static bool ImbueClassCheck(Item item)
		{
			if (item.active && (!item.accessory) && (item.ModItem is null or AORangedOrMeleeWeapon || ArcaneOdysseyConfig.Instance.AffectsOtherMods) && (!global::ArcaneOdyssey.ArcaneOdyssey.excludedItems.Contains(item.type)) && item.ammo == AmmoID.None)
			{
				return item.DamageType.Name == "TrueMeleeDamageClass" || item.DamageType.Name == "TrueMeleeNoSpeedDamageClass" || item.DamageType.Name == "MeleeRangedHybridDamageClass" ||
				item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.Ranged || item.DamageType == DamageClass.MeleeNoSpeed || (item.ModItem is not null && item.ModItem.GetType().IsSubclassOf(typeof(EmptyScroll)));
			}
			return false;
		}

		public static int FromAODefense(this int val) => (int)Math.Round(val/18f);

		public static int IndexOf<T>(this Array array, T item) => Array.IndexOf(array, item);

		public static bool TryGetImbue(this Item item, out Imbuable imbue)
		{
			imbue = item.ArcaneOdyssey().Imbue;
			return imbue is not null;
		}

		public static bool TryGetImbue(this Projectile projectile, out Imbuable imbue)
		{
			imbue = projectile.ArcaneOdyssey().Imbue;
			return imbue is not null;
		}

		public static bool TryGetImbue(this Player player, out Imbuable imbue)
		{
			imbue = player.ArcaneOdyssey().Imbue;
			return imbue is not null;
		}
		public static bool TryGetImbue(this ModPlayer player, out Imbuable imbue)
		{
			imbue = player.Player.ArcaneOdyssey().Imbue;
			return imbue is not null;
		}

		public static bool ServerOrSingleplayer => Main.netMode != NetmodeID.MultiplayerClient;

		public static bool AltUse(this Player player) => player.altFunctionUse == 2;
		

		public static bool PlayerHasImbue(this Imbuable imbue, Player player)
		{
			var type = imbue.GetType();
			if (imbue is SteamImbue steam)
			{
				type = steam.originalImbue.GetType();
			}
			return player.HasTypeInInventory(type);
		}

		/// <summary>
		/// Automatically generates localization, and formats statically
		/// </summary>
		/// <param name="mod">literally the mod</param>
		/// <param name="key">The localization key</param>
		/// <param name="formatting">Formatting args, not required</param>
		/// <returns></returns>
		public static LocalizedText CustomLocalization(this Mod mod, string key, params object[] formatting)
		{
			if (mod is not global::ArcaneOdyssey.ArcaneOdyssey)
			{
				mod = ModInstance;
			}
			LocalizedText text = LocalizedText.Empty;
			string fulllocalstuff = "";
			foreach (object format in formatting)
			{
				fulllocalstuff += " " + format;
			}
			if (global::ArcaneOdyssey.ArcaneOdyssey.staticLocalizer.TryGetValue(mod.GetLocalizationKey(key) + fulllocalstuff, out LocalizedText value))
			{
				text = value;
			}
			else
			{
				text = mod.GetLocalization(key, () => key.Split('.').LastOrDefault(key)).WithFormatArgs(formatting);
				global::ArcaneOdyssey.ArcaneOdyssey.staticLocalizer[mod.GetLocalizationKey(key) + fulllocalstuff] = text;
			}
			return text;
		}

		public static ArcaneOdyssey ModInstance => global::ArcaneOdyssey.ArcaneOdyssey.Instance;


		public static bool checklistfailed = false;
		private static int GetBossKillCount()
		{
			int count = 0;
			List<bool> conditions = [];
			if (checklistfailed || !ModLoader.TryGetMod("BossChecklist", out var checklist))
			{
				conditions.AddRange([DownedBosses.downedEvander, NPC.downedBoss1, NPC.downedBoss2, NPC.downedBoss3, NPC.downedQueenBee, NPC.downedSlimeKing, NPC.downedDeerclops, NPC.downedAncientCultist, NPC.downedChristmasIceQueen, NPC.downedChristmasSantank, NPC.downedClown, NPC.downedChristmasTree, NPC.downedEmpressOfLight, NPC.downedFishron, NPC.downedFrost, NPC.downedGoblins, NPC.downedGolemBoss, NPC.downedHalloweenKing, NPC.downedHalloweenTree, NPC.downedMartians, NPC.downedMechBoss1, NPC.downedMechBoss2, NPC.downedMechBoss3, NPC.downedMechBossAny, NPC.downedMoonlord, NPC.downedPlantBoss, NPC.downedPirates]);
				if (ModLoader.TryGetMod("CalamityMod", out var cal))
				{
					string[] extrBosses = "desertscourge giantclam crabulon hivemind perforator slimegod cryogen aquaticscourge cragmawmire brimstoneelemental calamitasclone greatsandshark anahitaleviathan astrumaureus plaguebringergoliath ravager astrumdeus guardians dragonfolly providence polterghast mauler nuclearterror oldduke ceaselessvoid stormweaver signus devourerofgods yharon exomechs calamitas primordialwyrm".Split(" ");
					foreach (var boss in extrBosses)
					{
						conditions.Add((bool)cal.Call("GetBossDowned", boss));
					}
				}
			}
			else
			{
				var raw = checklist.Call("GetBossInfoDictionary", ModInstance);
				if (raw is Dictionary<string, Dictionary<string, object>> data)
				{
					foreach (var boss in data)
					{
						bool isbossormini = (bool)boss.Value["isBoss"] || (bool)boss.Value["isMiniboss"];
						if (isbossormini)
						{
							var func = (Func<bool>)boss.Value["downed"];
							conditions.Add(func.Invoke());
						}
					}
				}
				else
				{
					checklistfailed = true;
					return GetBossKillCount();
				}
			}
			foreach (bool killed in conditions)
			{
				if (killed)
					count++;
			}
			checklistfailed = false;
			return count;
		}

		/// <summary>
		/// includes mini bosses
		/// </summary>
		public static int BossesKilled => GetBossKillCount();

		public static bool TryGetOwner(this Entity entity, out AOPlayer player)
		{
			var e = entity.TryGetOwner(out Player playr);
			if (playr.TryArcaneOdyssey(out player))
			{
				return e;
			}
			return false;
		}

		public static bool TryGetOwner(this Entity entity, out Player player)
		{
			player = null;
			if (entity is Projectile projectile)
			{
				player = Main.player[projectile.owner];
			}
			if (entity is NPC npc)
			{
				player = Main.player[npc.releaseOwner];
			}
			if (entity is Player player1)
			{
				player = player1;
			}
			return player is not null && player.active;
		}

		#region Enum Getters

		public static ItemType GetItemType(this Item item)
		{
			if (item.ModItem is AOBaseItem based && based.ItemType != ItemType.RESOLVESELF)
			{
				return based.ItemType;
			}
			if (item.vanity)
			{
				return ItemType.Vanity;
			}
			if (item.accessory)
			{
				return ItemType.Accessory;
			}
			if (item.bodySlot != -1 || item.legSlot != -1 || item.headSlot != -1 || item.wornArmor)
			{
				return ItemType.Armour;
			}
			if (item.axe > 0 || item.pick > 0)
			{
				return ItemType.Tool;
			}
			if (item.ammo != AmmoID.None)
			{
				return ItemType.Ammo;
			}
			if (item.DamageType == ModContent.GetInstance<SpiritDamage>())
			{
				return ItemType.Relic;
			}
			if (item.damage > 0 && item.useStyle != ItemUseStyleID.None)
			{
				return ItemType.Weapon;
			}
			if (item.material)
			{
				return ItemType.Material;
			}
			if (item.createTile != -1)
			{
				return ItemType.Block;
			}
			return ItemType.Item;
		}

		public static AORarities GetItemRare(this Item item)
		{
			if (item.ModItem is AOBaseItem based && based.AORarity != AORarities.RESOLVESELF)
			{
				return based.AORarity;
			}

			if (ModLoader.TryGetMod("CalamityMod", out Mod calamity))
			{
				if (item.rare == calamity.Find<ModRarity>("DarkOrange").Type)
				{
					return AORarities.Unknown;
				}
			}

			if (ModLoader.TryGetMod("NoxusBoss", out var wotg))
			{
				if (item.rare == wotg.Find<ModRarity>("SolynRewardRarity").Type)
				{
					return AORarities.Mystic;
				}
				if (item.rare == wotg.Find<ModRarity>("GenesisComponentRarity").Type)
				{
					return AORarities.Unknown;
				}
				if (item.rare == wotg.Find<ModRarity>("NamelessDeityRarity").Type)
				{
					return AORarities.Unknown;
				}
				if (item.rare == wotg.Find<ModRarity>("AvatarRarity").Type)
				{
					return AORarities.Unknown;
				}
			}
			if (item.questItem || item.rare == ItemRarityID.Quest)
			{
				return AORarities.Rare;
			}

			if (item.expert || item.rare == ItemRarityID.Expert)
			{
				return AORarities.Arcane;
			}
			if (item.master || item.rare == ItemRarityID.Master)
			{
				return AORarities.Mythical;
			}
			return item.rare switch
			{
				ItemRarityID.Gray => AORarities.Common,
				ItemRarityID.White => AORarities.Common,
				ItemRarityID.Blue => AORarities.Common,
				ItemRarityID.Green => AORarities.Uncommon,
				ItemRarityID.Orange => AORarities.Uncommon,
				ItemRarityID.LightRed => AORarities.Rare,
				ItemRarityID.Pink => AORarities.Rare,
				ItemRarityID.LightPurple => AORarities.Mystic,
				ItemRarityID.Lime => AORarities.Mystic,
				ItemRarityID.Yellow => AORarities.Arcane,
				ItemRarityID.Cyan => AORarities.Arcane,
				_ => AORarities.Mythical,
			};
		}
		#endregion

		#region structs and enums

		public struct WeaponAbility(Mod mod, string name, string description = "No description", Color? color = null)
		{
			public static string Key(Mod mod, string name)
			{
				return $"Mods.{mod.Name}.WeaponAbilities." + name.Replace(" ", null);
			}

			public string Name = name;
			public string Description = description;
			public Color? Colour = color;
			public Mod mod = mod;
			public string LocalizedName = Language.GetOrRegister(Key(mod, name) + ".DisplayName", () => name).Value;
			public string LocalizedDescription = Language.GetOrRegister(Key(mod, name) + ".Description", () => description).Value;


			public readonly TooltipLine GenerateTooltip()
			{
				string text = "";
				if (Colour.HasValue)
				{
					text += $"[c/{Colour.Value.Hex3()}:{mod.CustomLocalization("RandomWords.Ability").Value} - {LocalizedName}]";
				}
				else
				{
					text += $"{mod.CustomLocalization("RandomWords.Ability").Value} - {LocalizedName}";
				}
				text += $": {LocalizedDescription}";
				return new TooltipLine(mod, "AOAbility", text);
			}
		}

		/// <summary>
		/// Helper struct for set bonuses
		/// </summary>
		/// <param name="mod">This mod</param>
		/// <param name="name">The name of the set bonus</param>
		/// <param name="description">The description of this set bonus</param>
		/// <param name="otherItems">The internal names of the other two items in this set, head then body</param>
		/// <param name="colour">The colour of this set</param>
		public struct SetBonusHelper(Mod mod, string name, string description, string[] otherItems, Color? colour = null)
		{
			public Mod Mod = mod;
			public string Name = name;
			public string Description = description;
			public Color? Colour = colour;
			public string[] OtherItems = otherItems;

			public readonly string GenerateTooltip()
			{
				string text = "";
				if (Colour.HasValue)
				{
					text += $"[c/{Colour.Value.Hex3()}:{Name}]";
				}
				else
				{
					text += Name;
				}
				text += $" - {Description}";
				return text;
			}
		}

		public struct ImbueArmourStats(int size, int attkspeed, int power, int defence, int agility, int pierce)
		{
			public int Size = size;
			public int Attkspeed = attkspeed;
			public int Power = power;
			public int Pierce = pierce;
			public int Defence = defence;
			public int Agility = agility;

			public readonly ImbueArmourStats Corrected(Imbuable imbue)
			{
				if (imbue is FightingStyleBarred barred)
				{
					return new ImbueArmourStats(
						MathHelper.Lerp(0, Size, barred.BarValue / FightingStyleBarred.BarMax).Round(),
						MathHelper.Lerp(0, Attkspeed, barred.BarValue / FightingStyleBarred.BarMax).Round(),
						MathHelper.Lerp(0, Power, barred.BarValue / FightingStyleBarred.BarMax).Round(),
						MathHelper.Lerp(0, Defence, barred.BarValue / FightingStyleBarred.BarMax).Round(),
						MathHelper.Lerp(0, Agility, barred.BarValue / FightingStyleBarred.BarMax).Round(),
						MathHelper.Lerp(0, Pierce, barred.BarValue / FightingStyleBarred.BarMax).Round()
						);
				}
				return this;
			}
		}
		public enum ItemType
		{
			Block,
			Ammo,
			Item,
			Material,
			Accessory,
			Armour,
			Relic,
			Weapon,
			Tool,
			None,
			Vanity,
			RESOLVESELF
		}

		/// <summary>
		/// Arcane Odyssey rarities, converted to RarityID
		/// </summary>

		public enum AORarities
		{
			Unknown = ItemRarityID.Gray,
			Common = ItemRarityID.White,
			Uncommon = ItemRarityID.Green,
			Rare = ItemRarityID.LightRed,
			Mystic = ItemRarityID.LightPurple,
			Arcane = ItemRarityID.Yellow,
			Mythical = ItemRarityID.Red,
			Special,
			RESOLVESELF
		}

		public enum AOImbuableTier
		{
			Normal,
			Lost,
			Ancient,
			Primordial, // unused
			Developer,
		}

		/// <summary>
		/// Arcane Odyssey weapon tiers, used for scaling
		/// </summary>
		public enum AOItemTiers
		{
			/// <summary>
			/// Literally doesn't exist, don't bother
			/// </summary>
			None,
			/// <summary>
			/// Old weapons
			/// </summary>
			Poor,
			/// <summary>
			/// Bronze weapons
			/// </summary>
			Average,
			/// <summary>
			/// All the cool weapons
			/// </summary>
			Good, 
			/// <summary>
			/// Atleantean weapons+ use these, not in ao
			/// </summary>
			Great,

		}

		/// <summary>
		/// Represents an AO debuff
		/// </summary>
		/// <param name="debuffid">Terraria.ID.BuffID</param>
		/// <param name="duration">Duration, in ticks (60/second)</param>
		/// <param name="debuffRequiement">Damage% requirement to activate debuff</param>
		public struct AODebuffRequirement(int debuffid, int duration, int debuffRequiement = 0)
		{
			public int debuffPercent = debuffRequiement/100;
			public int debuffID = debuffid;
			public int debuffDuration = duration;
		}

		/// <summary>
		/// Magic status effects
		/// </summary>
		public struct SynergyEffects(int[] buffsToClear, MagicBuffMultiplier[] buffMultipliers)
		{
			public List<int> clearBuffs = [.. buffsToClear];
			public MagicBuffMultiplier[] magicBuffMultipliers = buffMultipliers;
			public readonly float MultiFromID(int id)
			{
				foreach (MagicBuffMultiplier multiplier in magicBuffMultipliers)
				{
					if (multiplier.buffID == id)
					{
						return multiplier.multiplier;
					}
				}
				return 1f;
			}
		}

		/// <summary>
		/// sahhhhhduiahyfoahgoaig
		/// </summary>
		/// <param name="requirement"></param>
		/// <param name="result"></param>
		/// <param name="duration"></param>
		public struct CombinedDebuff(int requirement, int result, int duration = 60)
		{
			public int requirement = requirement;
			public int result = result;
			public int duration = duration;
		}

		/// <summary>
		/// Damage multipliers from having debuffs interact
		/// </summary>
		/// <param name="buffid">Terraria.ID.BuffID</param>
		/// <param name="multi">Damage multipier (ex. 1.25f)</param>
		public struct MagicBuffMultiplier(int buffid, float multi)
		{
			public int buffID = buffid;
			public float multiplier = multi;
		}

		#endregion

		#region Random Math Functions
		public static int GetAOBuffStack(NPC npc, int index)
		{
			return (npc.buffTime[index] / 60 / 5) + 1;
		}

		/// <summary>
		/// Converts AO Galleons/Drachmae to Terraria Copper
		/// </summary>
		/// <param name="price">Price, in Galleons</param>
		/// <returns></returns>
		public static int GalleonToCopper(int price) => Item.buyPrice(silver: price);


		/// <summary>
		/// Converts AO weapon damage to Terraria damage. Scales very heavily with weapon tier
		/// </summary>
		/// <param name="AODamage">AO weapon damage multiplier</param>
		/// <param name="AOWeaponTier">AO weapon tier, use <see cref="AOItemTiers"/></param>
		/// <returns></returns>
		public static float WeaponDamage(AOItemTiers AOWeaponTier) => 25 * (int)AOWeaponTier;

		public static Vector2 Centre(this Gore gore, Vector2? newCentre)
		{
			if (newCentre.HasValue)
			{
				gore.position.X = newCentre.Value.X - (gore.Width / 2);
				gore.position.Y = newCentre.Value.Y - (gore.Height / 2);
				return gore.position;
			}
			else
				return new Vector2(gore.position.X - (gore.Width / 2), gore.position.Y - (gore.Height / 2));
		}

		/// <summary>
		/// Turns 1.4 into .6
		/// </summary>
		/// <param name="input">Input</param>
		/// <returns></returns>
		public static float FlipFloat(this float input)
		{
			if (input >= 2)
				return .01f;
			return 2f - input;
		}

		public static float MultiToPercent(this float multiplier) => multiplier-1f; // wow simplest function on the earth

		public static Vector2 SafeDirectionTo(this Entity entity, Vector2 destination, Vector2? defaultValue = null)
		{
			defaultValue ??= Vector2.Zero;
			return (destination - entity.Center).SafeNormalize(defaultValue.Value);
		}
		#endregion

		#region Player Inventory Helpers
		public static bool HasTypeInInventory(this Player player, Type type)
		{
			var no = new List<Item>(player.inventory);
			no.RemoveAll(e => e.ModItem is not Imbuable);
			foreach (var item in no)
			{
				if (item.ModItem.GetType().Name == type.Name || item.ModItem.GetType().IsSubclassOf(type))
				{
					return true;
				}
			}
			return false;
		}

		public static Item PlayerItem(this Player player)
		{
			if (Main.myPlayer == player.whoAmI && Main.mouseItem.active)
			{
				return Main.mouseItem;
			}
			else return player.HeldItem;
		}

		public static bool GetThisImbue(this Imbuable imbue, Player player)
		{
			if (player.TryGetImbue(out var playerimbue))
			{
				foreach (var item in player.inventory)
				{
					if (item.active)
					{
						if (item.Name == playerimbue.DisplayName.Value)
						{
							imbue = playerimbue;
							return true;
						}
					}
				}
			}
			return false;
		}
		#endregion

		#region ArcaneOdyssey()
		public static AOPlayer ArcaneOdyssey(this Player player) => player.GetModPlayer<AOPlayer>();
		public static AOPlayer ArcaneOdyssey(this ModPlayer player) => player.Player.GetModPlayer<AOPlayer>();
		public static bool TryArcaneOdyssey(this Player player, out AOPlayer playah) => player.TryGetModPlayer(out playah);
		public static bool TryArcaneOdyssey(this ModPlayer player, out AOPlayer playah) => player.Player.TryGetModPlayer(out playah);
		public static ArcaneNPC ArcaneOdyssey(this NPC npc) => npc.GetGlobalNPC<ArcaneNPC>();
		public static AOProjectile ArcaneOdyssey(this Projectile projectile) => projectile.GetGlobalProjectile<AOProjectile>();
		public static AOProjectile ArcaneOdyssey(this ModProjectile projectile) => projectile.Projectile.GetGlobalProjectile<AOProjectile>();
		public static AOItem ArcaneOdyssey(this Item item) => item.GetGlobalItem<AOItem>();

		public static IImbuableEntity AnyArcaneOdyssey(this Entity entity)
		{
			if (entity is Projectile projectile)
				return projectile.GetGlobalProjectile<AOProjectile>();
			if (entity is Player player)
				return player.GetModPlayer<AOPlayer>();
			if (entity is Item item)
				return item.GetGlobalItem<AOItem>();
			return null;
		}

		public static AOItem ArcaneOdyssey(this ModItem item) => item.Item.GetGlobalItem<AOItem>();
		public static bool TryArcaneOdyssey(this Item item, out AOItem result) => item.TryGetGlobalItem(out result);
		public static bool TryArcaneOdyssey(this ModItem item, out AOItem result) => item.Item.TryGetGlobalItem(out result);
		#endregion
	}

	public interface IImbuableEntity
	{
		public Imbuable Imbue { get; set; }
	}
}
