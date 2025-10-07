using ArcaneOdyssey.Content.Items;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Projectiles;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.VFX.Rarities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArcaneOdyssey
{
	public static class AOUtils
	{
		public static int GetAOBuffStack(NPC npc, int index)
		{
			return (npc.buffTime[index] / 60 / 5) + 1;
		}
		public static Vector2 GetDrawOriginCentre(this Entity entity) => new(entity.width / 2, entity.height / 2);

		public static Imbuable Imbue(this Player player) => player.ArcaneOdyssey().imbue;
		public static Imbuable Imbue(this Projectile projectile) => projectile.ArcaneOdyssey().imbue;
		public static Imbuable Imbue(this Item item) => item.ArcaneOdyssey().imbue;

		public static int Round(this float num) => (int)Math.Round(num);

		public static void Kill(this Entity entity)
		{
			if (entity is Projectile projectile)
			{
				projectile.Kill();
			}
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
				if (item.ModItem is Imbuable imbuable)
				{
					imbues.Add(imbuable);
				}
			}
			return imbues;
		}

		public static bool ImbueClassCheck(Projectile projectile)
		{
			if ((projectile.ModProjectile is null or AOPlayerProjectile || ArcaneOdysseyConfig.Instance.AffectsOtherMods) && (!global::ArcaneOdyssey.ArcaneOdyssey.ExcludedProjectiles.Contains(projectile.type)))
			{
				List<string> goodclasses = new(["TrueMeleeDamageClass", "TrueMeleeNoSpeedDamageClass", "MeleeRangedHybridDamageClass"]);
				if (goodclasses.Contains(projectile.DamageType.Name))
				{
					return true;
				}
				return (projectile.DamageType == DamageClass.Melee || projectile.DamageType == DamageClass.Ranged || projectile.ModProjectile is MagicSpell or StrengthTechnique || projectile.DamageType == DamageClass.MeleeNoSpeed) && projectile.ModProjectile is not MagicCircle1 or MagicCircle2
					&& projectile.owner != 255 && !projectile.hostile && !projectile.npcProj && projectile.type != ProjectileID.FallingStar;
			}
			return false;
		}

		public static bool ImbueClassCheck(Item item)
		{
			if (!item.accessory && (item.ModItem is null or AORangedOrMeleeWeapon || ArcaneOdysseyConfig.Instance.AffectsOtherMods) && (!global::ArcaneOdyssey.ArcaneOdyssey.ExcludedItems.Contains(item.type)) && item.ammo == AmmoID.None)
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
			imbue = item.ArcaneOdyssey().imbue;
			return imbue is not null;
		}

		public static bool TryGetImbue(this Projectile projectile, out Imbuable imbue)
		{
			imbue = projectile.ArcaneOdyssey().imbue;
			return imbue is not null;
		}

		public static bool TryGetImbue(this Player player, out Imbuable imbue)
		{
			imbue = player.ArcaneOdyssey().imbue;
			return imbue is not null;
		}
		public static bool TryGetImbue(this ModPlayer player, out Imbuable imbue)
		{
			imbue = player.Player.ArcaneOdyssey().imbue;
			return imbue is not null;
		}

		public static bool ServerOrSingleplayer => Main.dedServ || Main.netMode == NetmodeID.SinglePlayer;

		public static bool AltUse(this Player player) => player.altFunctionUse == 2;
		

		public class WeaponAbility(Mod mod, string name, string description, Color? color = null)
		{
			private readonly string Name = name;
			private readonly string Description = description;
			private readonly Color? Colour = color;
			private readonly Mod mod = mod;

			public TooltipLine GenerateTooltip()
			{
				string text = "";
				if (Colour.HasValue)
				{
					text += $"[c/{Colour.Value.Hex3()}:{mod.CustomLocalization("RandomWords.Ability").Value} - {Name}]";
				}
				else
				{
					text += $"{mod.CustomLocalization("RandomWords.Ability").Value} - {Name}";
				}
				text += $": {Description}";
				return new TooltipLine(mod, "AOAbility", text);
			}
		}

		public static bool PlayerHasImbue(this Imbuable imbue, Player player, List<Imbuable> imbues = null)
		{
			imbues ??= player.GetAllImbues();
			var postcheck = false;
			foreach (Imbuable imb in imbues)
			{
				if (imbue is SteamImbue steam)
				{
					postcheck |= imb.Name == steam.originalImbue.Name;
				}
				else if (imbue is not null)
				{
					postcheck |= imb.Name == imbue.Name;
				}
			}
			return postcheck;
		}

		/// <summary>
		/// Automatically generates localization, and formats statically
		/// </summary>
		/// <param name="mod">literally the mod</param>
		/// <param name="key">The localization key</param>
		/// <param name="formatting">Formatting args, not required</param>
		/// <returns></returns>
		public static LocalizedText CustomLocalization(this Mod mod, string key, object[] formatting = null)
		{
			LocalizedText text = LocalizedText.Empty;
			string fulllocalstuff = "";
			if (formatting is not null && formatting.Length > 0)
			{
				foreach (object format in formatting)
				{
					fulllocalstuff += " " + format;
				}
			}
			if (global::ArcaneOdyssey.ArcaneOdyssey.staticLocalizer.TryGetValue(mod.GetLocalizationKey(key) + fulllocalstuff, out LocalizedText value))
			{
				text = value;
			}
			else
			{
				text = mod.GetLocalization(key, () => key.Split('.').LastOrDefault(key));
				if (formatting is not null)
				{
					text = text.WithFormatArgs(formatting);
				}
				global::ArcaneOdyssey.ArcaneOdyssey.staticLocalizer[mod.GetLocalizationKey(key) + fulllocalstuff] = text;
			}
			return text;
		}

		private static int GetBossKillCount()
		{
			int count = 0;
			bool[] conditions = [DownedBosses.downedEvander, NPC.downedBoss1, NPC.downedBoss2, NPC.downedBoss3, NPC.downedQueenBee, NPC.downedSlimeKing, NPC.downedDeerclops, NPC.downedAncientCultist, NPC.downedChristmasIceQueen, NPC.downedChristmasSantank, NPC.downedClown, NPC.downedChristmasTree, NPC.downedEmpressOfLight, NPC.downedFishron, NPC.downedFrost, NPC.downedGoblins, NPC.downedGolemBoss, NPC.downedHalloweenKing, NPC.downedHalloweenTree, NPC.downedMartians, NPC.downedMechBoss1, NPC.downedMechBoss2, NPC.downedMechBoss3, NPC.downedMechBossAny, NPC.downedMoonlord, NPC.downedPlantBoss, NPC.downedPirates];
			foreach (bool killed in conditions)
			{
				if (killed)
					count++;
			}
			return count;
		}

		public static ItemType GetItemType(this Item item)
		{
			if (item.ModItem is AOBaseItem based && based.ItemType != ItemType.RESOLVESELF)
			{
				return based.ItemType;
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
			if (item.DamageType == DamageClass.MagicSummonHybrid)
			{
				return ItemType.Relic;
			}
			if (item.damage != -1)
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
			if (item.expert || item.rare == ItemRarityID.Expert)
			{
				return AORarities.Arcane;
			}
			if (item.master || item.rare == ItemRarityID.Master)
			{
				return AORarities.Zenith;
			}
			switch (item.rare)
			{
				case -1:
					return AORarities.Common;
					break;
				case 0:
					return AORarities.Common;
					break;
				case 1:
					return AORarities.Common;
					break;
				case 2:
					return AORarities.Uncommon;
					break;
				case 3:
					return AORarities.Uncommon;
					break;
				case 4:
					return AORarities.Rare;
					break;
				case 5:
					return AORarities.Rare;
					break;
				case 6:
					return AORarities.Exotic;
					break;
				case 7:
					return AORarities.Exotic;
					break;
				case 8:
					return AORarities.Arcane;
					break;
				case 9:
					return AORarities.Arcane;
					break;
				default:
					return AORarities.Zenith;
					break;
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
			RESOLVESELF
		}

		/// <summary>
		/// includes mini bosses
		/// </summary>
		public static int BossesKilled => GetBossKillCount();

		/// <summary>
		/// Arcane Odyssey rarities, converted to RarityID
		/// </summary>
		public enum AORarities
		{
			Common = ItemRarityID.Gray,
			Uncommon = ItemRarityID.White,
			Rare = ItemRarityID.Blue,
			Exotic = ItemRarityID.LightRed,
			Arcane = ItemRarityID.Lime,
			Zenith = ItemRarityID.Master,
			Special,
			RESOLVESELF
		}

		public enum AOImbuableTier
		{
			Unobtainable,
			Normal,
			Lost,
			Ancient,
			Custom,
		}

		/// <summary>
		/// Arcane Odyssey weapon tiers, used for scaling
		/// </summary>
		public enum AOWeaponTiers
		{
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
			/// Atleantean weapons+ use these
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

		public static Vector2 Centre(this Gore gore, Vector2? newCentre)
		{
			if (newCentre.HasValue)
			{
				gore.position.X = (newCentre.Value.X - gore.Width * gore.scale / 2);
				gore.position.Y = (newCentre.Value.Y - gore.Height * gore.scale / 2);
				return gore.position;
			}
			else
				return new Vector2(gore.position.X - (gore.Width * gore.scale / 2), gore.position.Y - (gore.Height * gore.scale / 2));
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

		/// <summary>
		/// Converts AO Galleons/Drachmae to Terraria Copper
		/// </summary>
		/// <param name="price">Price, in Galleons</param>
		/// <returns></returns>
		public static int GalleonToCopper(int price) => price * 100; // very simple lol, previously nothing was worth anything


		/// <summary>
		/// Converts AO weapon damage to Terraria damage. Scales very heavily with weapon tier
		/// </summary>
		/// <param name="AODamage">AO weapon damage multiplier</param>
		/// <param name="AOWeaponTier">AO weapon tier, use <see cref="AOWeaponTiers"/></param>
		/// <returns></returns>
		public static float WeaponDamage(AOWeaponTiers AOWeaponTier) => 25 * ((int)AOWeaponTier+1);

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

		public static AOPlayer ArcaneOdyssey(this Player player) => player.GetModPlayer<AOPlayer>();
		public static ArcaneNPC ArcaneOdyssey(this NPC npc) => npc.GetGlobalNPC<ArcaneNPC>();
		public static AOProjectile ArcaneOdyssey(this Projectile projectile) => projectile.GetGlobalProjectile<AOProjectile>();
		public static AOItem ArcaneOdyssey(this Item item) => item.GetGlobalItem<AOItem>();
	}
}
