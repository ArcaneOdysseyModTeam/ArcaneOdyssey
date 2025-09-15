using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Projectiles;
using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
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
		public static bool ImbueClassCheck(Projectile projectile)
		{
			if (projectile.ModProjectile is null or AOPlayerProjectile || ArcaneOdysseyConfig.Instance.AffectsOtherMods)
			{
				List<string> goodclasses = new(["TrueMeleeDamageClass", "TrueMeleeNoSpeedDamageClass", "MeleeRangedHybridDamageClass"]);
				if (goodclasses.Contains(projectile.DamageType.Name))
				{
					return true;
				}
				return (projectile.DamageType == DamageClass.Melee || projectile.DamageType == DamageClass.Ranged || projectile.ModProjectile is MagicSpell || projectile.DamageType == DamageClass.MeleeNoSpeed) && projectile.ModProjectile is not MagicCircle or MagicCircle2
					&& projectile.owner != 255 && !projectile.hostile && !projectile.npcProj && projectile.type != ProjectileID.FallingStar;
			}
			return false;
		}

        public static bool ImbueClassCheck(Item item)
        {
            if (item.ModItem is null or AOWeapon || ArcaneOdysseyConfig.Instance.AffectsOtherMods)
            {
				string[] goodclasses = ["TrueMeleeDamageClass", "TrueMeleeNoSpeedDamageClass", "MeleeRangedHybridDamageClass"];
				if (goodclasses.Contains(item.DamageType.Name))
				{
					return true;
				}
				return item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.Ranged || item.DamageType == DamageClass.MeleeNoSpeed || (item.ModItem is not null && item.ModItem.GetType().IsSubclassOf(typeof(DefaultScroll)));
			}
			return false;
		}

        public static int FromAODefense(this int val)
		{
			return (int)Math.Round(val/18f);
		}

		public static int IndexOf<T>(this Array array, T item)
		{
			return Array.IndexOf(array, item);
		}

		public static bool TryGetImbue(this Item item, Player player, out AOMagic imbue)
		{
			imbue = null;
            if ((item.ModItem is null or AOWeapon || ArcaneOdysseyConfig.Instance.AffectsOtherMods) && ImbueClassCheck(item))
            {
				imbue ??= player.AOPlayer().imbue;
            }
			return imbue is not null;
        }

		public static bool TryGetImbue(this Projectile projectile, Player player, out AOMagic imbue)
		{
			imbue = null;
			if (projectile.ModProjectile is AOPlayerProjectile proj && ImbueClassCheck(projectile))
			{
				imbue ??= proj.thisMagic;
			}
			else if ((projectile.ModProjectile is null || ArcaneOdysseyConfig.Instance.AffectsOtherMods) && ImbueClassCheck(projectile))
			{
				imbue ??= player.AOPlayer().imbue;
			}
			return imbue is not null;
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
			if (ArcaneOdyssey.staticLocalizer.TryGetValue(mod.GetLocalizationKey(key) + (formatting is not null ? " " + formatting[0] : ""), out LocalizedText value))
			{
				text = value;
			}
			else
			{
				text = mod.GetLocalization(key);
				if (formatting is not null)
				{
					text = text.WithFormatArgs(formatting);
				}
				ArcaneOdyssey.staticLocalizer[mod.GetLocalizationKey(key) + (formatting is not null ? " " + formatting[0] : "")] = text;
			}
			return text;
		}

		public static int BonusBossKills()
		{
			int count = 0;
			bool[] conditions = [NPC.downedBoss1, NPC.downedBoss2, NPC.downedBoss3, NPC.downedQueenBee, NPC.downedSlimeKing, NPC.downedDeerclops, NPC.downedAncientCultist, NPC.downedChristmasIceQueen, NPC.downedChristmasSantank, NPC.downedClown, NPC.downedChristmasTree, NPC.downedEmpressOfLight, NPC.downedFishron, NPC.downedFrost, NPC.downedGoblins, NPC.downedGolemBoss, NPC.downedHalloweenKing, NPC.downedHalloweenTree, NPC.downedMartians, NPC.downedMechBoss1, NPC.downedMechBoss2, NPC.downedMechBoss3, NPC.downedMechBossAny, NPC.downedMoonlord, NPC.downedPlantBoss, NPC.downedPirates];
			foreach (bool killed in conditions)
			{
				if (killed)
					count++;
			}
			return count;
		}

		/// <summary>
		/// Arcane Odyssey rarities, converted to RarityID
		/// </summary>
		public class AORarities
		{
			public const short Common = -1;
			public const short Uncommon = 0;
			public const short Rare = 1;
			public const short Exotic = 4;
			public const short Legendary = 7;
		}

		public enum AOMagicTier
		{
			Normal = 1,
			Lost = 2,
			Ancient = 3,
			Custom = 4,
		}

		/// <summary>
		/// Arcane Odyssey weapon tiers, used for scaling. Weapon skill index: 2 is Old; 3 is Normal; 5 is Excellent
		/// </summary>
		public class AOWeaponTiers
		{
			public const short Old = 1;
			public const short Normal = 2;
			public const short Excellent = 3;
		}

		/// <summary>
		/// Represents an AO debuff
		/// </summary>
		/// <param name="debuffid">Terraria.ID.BuffID</param>
		/// <param name="duration">Duration, in ticks (60/second)</param>
		/// <param name="debuffRequiement">Damage% requirement to activate debuff</param>
		public class AODebuffRequirement(int debuffid, int duration, int? debuffRequiement = null)
		{
			public int debuffID = debuffid;
			public int debuffDuration = duration;
			public int? DebuffPercent
			{
				get
				{
					if (debuffRequiement is not null)
						return debuffRequiement / 100;
					return null;
				}
			}
		}

		/// <summary>
		/// Magic status effects
		/// </summary>
		/// <param name="buffsToClear">Buffs this magic clears on hit</param>
		/// <param name="buffMultipliers">Damage multipliers from having debuffs interact</param>
		public class MagicEffects(int[] buffsToClear, MagicBuffMultiplier[] buffMultipliers)
		{
			public int[] clearBuffs = buffsToClear;
			public MagicBuffMultiplier[] magicBuffMultipliers = buffMultipliers;

			public float MultiFromID(int id)
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
		/// 
		/// </summary>
		/// <param name="requirement"></param>
		/// <param name="result"></param>
		/// <param name="duration"></param>
		public class CombinedDebuff(int requirement, int result, int duration = 60)
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
		public class MagicBuffMultiplier(int buffid, float multi)
		{
			public int buffID = buffid;
			public float multiplier = multi;
		}

		/// <summary>
		/// Converts AO Galleons/Drachmae to Terraria Copper
		/// </summary>
		/// <param name="price">Price, in Galleons</param>
		/// <param name="rarity">Rarity of the item, use AORarities</param>
		/// <returns></returns>
		public static int GalleonToCopper(int price, int rarity)
		{
			return price * (rarity + 2) * (1 + 1 / 9);
		}


		/// <summary>
		/// Converts AO weapon damage to Terraria damage. Scales very heavily with weapon tier
		/// </summary>
		/// <param name="AODamage">AO weapon damage multiplier</param>
		/// <param name="AOWeaponTier">AO weapon tier, use AOWeaponTiers</param>
		/// <returns></returns>
		public static float WeaponDamage(int AOWeaponTier) => 25 * AOWeaponTier;

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

		public static float MultiToPercent(this float multiplier)
		{
			return multiplier-1f; // wow simplest function on the earth
		}

		public static Vector2 SafeDirectionTo(this Entity entity, Vector2 destination, Vector2? defaultValue = null)
		{
			defaultValue ??= Vector2.Zero;
			return (destination - entity.Center).SafeNormalize(defaultValue.Value);
		}

		public static AOPlayer AOPlayer(this Player player) => player.GetModPlayer<AOPlayer>();
	}
}