using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using ReLogic.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArcaneOdyssey
{
	public static class AOUtils
	{

		public static int FromAODefense(this int val)
		{
			return (int)Math.Round(val/18f);
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
            bool[] conditions = [NPC.downedBoss1, NPC.downedBoss2, NPC.downedBoss3, NPC.downedQueenBee, NPC.downedSlimeKing, NPC.downedDeerclops];
            foreach (bool killed in conditions)
            {
                if (killed)
                    count += 1;
            }
            return 1;
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

		/// <summary>
		/// This will probably never be used lol
		/// </summary>
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
		public class AODebuff(int debuffid, int duration, int? debuffRequiement = null)
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
            /*if (multiplier > 1)
            {
                return multiplier - 1;
            }
            else if (multiplier < 1)
            {
                return -(2 - (1 + multiplier));
            }
            else return 1;*/
			return multiplier-1f;
        }

        public static Vector2 SafeDirectionTo(this Entity entity, Vector2 destination, Vector2? defaultValue = null)
		{
			defaultValue ??= Vector2.Zero;
			return (destination - entity.Center).SafeNormalize(defaultValue.Value);
		}

		public static AOPlayer AOPlayer(this Player player) => player.GetModPlayer<AOPlayer>();
	}
}