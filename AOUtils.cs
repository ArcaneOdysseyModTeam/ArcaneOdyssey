using ArcaneOdyssey.Content.Items.Base;
using ReLogic.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ArcaneOdyssey
{
	public class AOUtils
	{
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
		public class AOMagicTier
		{
			public const short Normal = 1;
			public const short Lost = 2;
			public const short Ancient = 3;
			public const short Custom = 4;
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
		public static float FlipFloat(float input)
		{
			if (input >= 2)
				return .01f;
			return 2f - input;
		}

		public static float MultiToPercent(float multiplier)
		{
			if (multiplier > 1)
			{
				return 1 - multiplier;
			}
			else if (multiplier < 1)
			{
				return -(2 - (1 + multiplier));
			}
			else return 1;
		}
		public static Vector2 EntitySafeDirectionTo(Entity entity, Vector2 destination, Vector2? fallback = null)
		{
			fallback ??= Vector2.Zero;
			return (destination - entity.Center).SafeNormalize(fallback.Value);
		}
	}
}