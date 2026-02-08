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
		public Imbuable Imbue { get => Item.ArcaneOdyssey()?.Imbue; set => Item.ArcaneOdyssey().Imbue = value; }

		/// <summary>
		/// Base value
		/// </summary>
		public virtual int AODefense => 0;

		/// <summary>
		/// Base value
		/// </summary>
		public virtual int AOAgility => 0;

		/// <summary>
		/// Base value
		/// </summary>
		public virtual int AOAttkSpd => 0;

		/// <summary>
		/// Base value
		/// </summary>
		public virtual int AOValue => 0;

		/// <summary>
		/// Base value
		/// </summary>
		public virtual int AOSize => 0;

		/// <summary>
		/// Base value
		/// </summary>
		public virtual int AOPierce => 0;

		/// <summary>
		/// Base value
		/// </summary>
		public virtual int AOPower => 0;

		/// <summary>
		/// Base value
		/// </summary>
		public virtual int Haste => 0;

		public virtual int MinionSlots => Set.HasValue ? (int)ArmourTier / 2 : 0;

		public virtual int AOMaxMana => 0;


		/// <summary>
		/// Should only be set on boots
		/// </summary>
		public virtual SetBonusHelper? Set => null;

		public virtual void ArmorSetEffects(Player player) { }

		public virtual bool? Arcanium => null;

		public abstract AOItemTiers ArmourTier { get; }


		public override void UpdateArmorSet(Player player)
		{
			if (Set.HasValue)
			{
				player.setBonus = Set.Value.Tooptip;
				ArmorSetEffects(player);
			}
		}

		public int GetArmourSizeStat()
		{
			int val = AOSize;
			if (Imbue is not null)
				val += Imbue.ArmourStats.Value.Corrected(Imbue).Size * (int)ArmourTier;
			return val;
		}

		public int GetArmourHasteStat()
		{
			int val = Haste;
			if (Imbue is not null)
				val += Imbue.ArmourStats.Value.Corrected(Imbue).Haste * (int)ArmourTier;
			return val;
		}

		public int GetArmourAgilityStat()
		{
			int val = AOAgility;
			if (Imbue is not null)
				val += Imbue.ArmourStats.Value.Corrected(Imbue).Agility * (int)ArmourTier;
			return val;
		}

		public int GetArmourPierceStat()
		{
			int val = AOPierce;
			if (Imbue is not null)
				val += Imbue.ArmourStats.Value.Corrected(Imbue).Pierce * (int)ArmourTier;
			return val;
		}

		public int GetArmourPowerStat()
		{
			int val = AOPower;
			if (Imbue is not null)
				val += Imbue.ArmourStats.Value.Corrected(Imbue).Power * (int)ArmourTier;
			return val;
		}

		public int GetArmourAttkSpeedStat()
		{
			int val = AOAttkSpd;
			if (Imbue is not null)
				val += Imbue.ArmourStats.Value.Corrected(Imbue).Attkspeed * (int)ArmourTier;
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

			if (AOMaxMana > 0)
			{
				tooltips.AddTooltip(new(Mod, "AOMaxMana", Mod.CustomLocalization("ArmourAutoTooltip.Mana", AOMaxMana).Value));
			}
			if (MinionSlots > 0)
			{
				tooltips.AddTooltip(new(Mod, "MinionSlots", Mod.CustomLocalization("ArmourAutoTooltip.Minions", MinionSlots).Value));
			}
			if (AOAgility > 0)
			{
				tooltips.AddTooltip(new(Mod, "AOAgility", Mod.CustomLocalization("ArmourAutoTooltip.Agility", Math.Round(GetArmourAgilityStat() / 10f)).Value));
			}
			if (AOSize > 0)
			{
				tooltips.AddTooltip(new(Mod, "AOSize", Mod.CustomLocalization("ArmourAutoTooltip.Size", Math.Round(GetArmourSizeStat() / 3f)).Value));
			}
			if (AOPower > 0)
			{
				tooltips.AddTooltip(new(Mod, "AOPower", Mod.CustomLocalization("ArmourAutoTooltip.Power", AOPower).Value));
			}
			if (AOAttkSpd > 0)
			{
				tooltips.AddTooltip(new(Mod, "AOAttkSpd", Mod.CustomLocalization("ArmourAutoTooltip.Speed", Math.Round(GetArmourAttkSpeedStat() / 3f)).Value));
			}
			if (AOPierce > 0)
			{
				tooltips.AddTooltip(new(Mod, "AOPierce", Mod.CustomLocalization("ArmourAutoTooltip.Pierce", GetArmourPierceStat() / 5).Value));
			}
		}

		public override void UpdateEquip(Player player)
		{
			if (Arcanium.HasValue && player.TryGetImbue(out Imbuable imbue) && imbue.ArmourStats.HasValue)
			{
				if ((Arcanium.Value && imbue is AOMagic) || ((!Arcanium.Value) && imbue is FightingStyle))
				{
					Item.defense = AODefense.FromAODefense() + imbue.ArmourStats.Value.Corrected(imbue).Defence.FromAODefense();
				}
			}
			player.moveSpeed += GetArmourAgilityStat() / 100f;
			player.GetDamage(DamageClass.Generic) += GetArmourPowerStat() / 50f;
			player.GetCritChance(DamageClass.Generic) += GetArmourPowerStat();
			player.ArcaneOdyssey().AOSizeStat += GetArmourSizeStat();
			player.ArcaneOdyssey().AOHasteStat += GetArmourHasteStat();
			player.GetArmorPenetration(DamageClass.Generic) += GetArmourPierceStat() / 5;
			player.GetAttackSpeed(DamageClass.Generic) += GetArmourAttkSpeedStat() / 275;
			player.maxMinions += MinionSlots;
			player.statManaMax2 += AOMaxMana;
		}

		public override void SetStaticDefaults()
		{
			_ = Set?.Tooptip;
			base.SetStaticDefaults();
		}
	}
}