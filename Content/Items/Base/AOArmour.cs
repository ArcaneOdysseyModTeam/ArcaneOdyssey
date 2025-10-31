using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Base
{
	/// <summary>
	/// also works as an accessory
	/// </summary>
	public abstract class AOArmour : AOBaseItem, ILocalizedModType
	{
		public override string LocalizationCategory => Item.accessory ? "Items.Accessories" : "Items.Armour";

		/// <summary>
		/// At max item level btw
		/// </summary>
		public virtual int AODefense => 0;

		/// <summary>
		/// At max item level btw
		/// </summary>
		public virtual int AOAgility => 0;

		/// <summary>
		/// At max item level btw
		/// </summary>
		public virtual int AOAttkSpd => 0;

		/// <summary>
		/// Base value
		/// </summary>
		public virtual int AOValue => 0;

		/// <summary>
		/// At max item level btw
		/// </summary>
		public virtual int AOSize => 0;

		/// <summary>
		/// At max item level btw
		/// </summary>
		public virtual int AOPierce => 0;

		/// <summary>
		/// At max item level btw
		/// </summary>
		public virtual int AOPower => 0;

		/// <summary>
		/// Without enchantments ect
		/// </summary>

		public virtual int MinionSlots => 0;

		public virtual int MaxMana => 0;

		/// <summary>
		/// Should only be set on boots
		/// </summary>
		public virtual SetBonusHelper? Set => null;

		public virtual void ArmorSetEffects(Player player) {}

		public virtual bool? Arcanium => null;

		public abstract AOItemTiers ArmourTier { get; }


		public override void UpdateArmorSet(Player player)
		{
			if (Set.HasValue)
			{
				player.setBonus = Set.Value.GenerateTooltip();
				ArmorSetEffects(player);
			}
		}

		public int GetArmourSizeStat()
		{
			int val = AOSize;
			if (this.Imbue() is not null)
				val += this.Imbue().ArmourStats.Value.Corrected(this.Imbue()).Size * (int)ArmourTier;
			return val;
		}

		public int GetArmourAgilityStat()
		{
			int val = AOAgility;
			if (this.Imbue() is not null)
				val += this.Imbue().ArmourStats.Value.Corrected(this.Imbue()).Agility * (int)ArmourTier;
			return val;
		}

		public int GetArmourPierceStat()
		{
			int val = AOPierce;
			if (this.Imbue() is not null)
				val += this.Imbue().ArmourStats.Value.Corrected(this.Imbue()).Pierce * (int)ArmourTier;
			return val;
		}

		public int GetArmourPowerStat()
		{
			int val = AOPower;
			if (this.Imbue() is not null)
				val += this.Imbue().ArmourStats.Value.Corrected(this.Imbue()).Power * (int)ArmourTier;
			return val;
		}

		public int GetArmourAttkSpeedStat()
		{
			int val = AOAttkSpd;
			if (this.Imbue() is not null)
				val += this.Imbue().ArmourStats.Value.Corrected(this.Imbue()).Attkspeed * (int)ArmourTier;
			return val;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			if (head.ModItem is not null && body.ModItem is not null && Set.HasValue)
			{
				return head.ModItem.Name == Set.Value.OtherItems[0] && body.ModItem.Name == Set.Value.OtherItems[1];
			}
			return false;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.defense = AODefense.FromAODefense();
			Item.value = GalleonToCopper(AOValue);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (tooltips.Contains(tooltips.Find(e => e.Name == "Social")))
				return;
			tooltips.Reverse();
			var index = tooltips.IndexOf(tooltips.Find(e => e.Name == "Defense" || e.Name == "Equipable" || e.Name == "FavoriteDesc" || e.Name == "ItemName" || e.Name.StartsWith("Tooltip"))) - 1;
			if (index < 0)
				index = 0;
			int lasttooltip = 0;
			if (tooltips.Contains(tooltips.Find(e => e.Name.StartsWith("Tooltip"))))
				lasttooltip = Convert.ToInt32(tooltips.Find(e => e.Name.StartsWith("Tooltip")).Name.Replace("Tooltip", null));
			if (MaxMana > 0)
			{
				tooltips.Insert(index, new(Mod, "Tooltip" + ++lasttooltip, Mod.CustomLocalization("ArmourAutoTooltip.Mana", [MaxMana]).Value));
			}
			if (MinionSlots > 0)
			{
				tooltips.Insert(index, new(Mod, "Tooltip" + ++lasttooltip, Mod.CustomLocalization("ArmourAutoTooltip.Minions", [MinionSlots]).Value));
			}
			if (AOAgility > 0)
			{
				tooltips.Insert(index, new(Mod, "Tooltip" + ++lasttooltip, Mod.CustomLocalization("ArmourAutoTooltip.Agility", [Math.Round(GetArmourAgilityStat() / 10f)]).Value));
			}
			if (AOSize > 0)
			{
				tooltips.Insert(index, new(Mod, "Tooltip" + ++lasttooltip, Mod.CustomLocalization("ArmourAutoTooltip.Size", [Math.Round(GetArmourSizeStat() / 3f)]).Value));
			}
			if (AOPower > 0)
			{
				tooltips.Insert(index, new(Mod, "Tooltip" + ++lasttooltip, Mod.CustomLocalization("ArmourAutoTooltip.Power", [AOPower]).Value));
			}
			if (AOAttkSpd > 0)
			{
				tooltips.Insert(index, new(Mod, "Tooltip" + ++lasttooltip, Mod.CustomLocalization("ArmourAutoTooltip.Speed", [Math.Round(GetArmourAttkSpeedStat() / 3f)]).Value));
			}
			if (AOPierce > 0)
			{
				tooltips.Insert(index, new(Mod, "Tooltip" + ++lasttooltip, Mod.CustomLocalization("ArmourAutoTooltip.Pierce", [GetArmourPierceStat()/5]).Value));
			}
			tooltips.Reverse();
		}

		public override void UpdateEquip(Player player)
		{
			if (Arcanium.HasValue && player.TryGetImbue(out Imbuable imbue) && imbue.ArmourStats.HasValue)
			{
				if ((Arcanium.Value && imbue is AOMagic) || ((!Arcanium.Value) && imbue is FightingStyle))
				{
					Item.defense = AODefense.FromAODefense() + imbue.ArmourStats.Value.Corrected(imbue).Defence;
				}
			}
			player.moveSpeed += GetArmourAgilityStat() / 100f;
			player.GetDamage(DamageClass.Generic) += GetArmourPowerStat() / 100f;
			player.GetCritChance(DamageClass.Generic) += GetArmourPowerStat();
			player.ArcaneOdyssey().AOSizeStat += GetArmourSizeStat();
			player.GetArmorPenetration(DamageClass.Generic) += GetArmourPierceStat() / 5;
			player.GetAttackSpeed(DamageClass.Generic) += GetArmourAttkSpeedStat() / 300;
			player.maxMinions += MinionSlots;
			player.statManaMax2 += MaxMana;
		}
	}
}