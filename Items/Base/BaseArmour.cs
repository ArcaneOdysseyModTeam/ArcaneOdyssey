using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Debug;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Base
{
	/// <summary>
	/// also works as an accessory
	/// </summary>
	public abstract class BaseArmour : BaseItem, IImbuable
	{
		public const float SizeDivision = 2.75f;
		public const float HasteDivision = 2f;

		public Imbuable Imbue { get => Item.ArcaneOdyssey()?.Imbue; set => Item.ArcaneOdyssey().Imbue = value; }

		public bool? BenifitsFromScrollStats => Arcanium;

		/// <summary>
		/// Base value
		/// </summary>
		public virtual ushort AODefense => 0;

		/// <summary>
		/// Base value
		/// </summary>
		public virtual short AOAgility => 0;

		/// <summary>
		/// Base value
		/// </summary>
		public virtual short AOAttkSpd => 0;

		/// <summary>
		/// Base value
		/// </summary>
		public virtual short Size => 0;

		/// <summary>
		/// Base value
		/// </summary>
		public virtual short AOPierce => 0;

		/// <summary>
		/// Base value
		/// </summary>
		public virtual short AOPower => 0;

		/// <summary>
		/// Base value
		/// </summary>
		public virtual short Haste => 0;

		public virtual byte MinionSlots => (byte)(Set.HasValue ? (int)ArmourTier / 2 : 0);

		public virtual byte MaxMana => 0;


		/// <summary>
		/// Should only be set on chest
		/// </summary>
		public virtual SetBonusHelper? Set => null;

		public SetBonusHelper GetSetBonusHelper(params string[] items) => new(this, items);

		public virtual void ArmorSetEffects(Player player) { }

		public virtual bool? Arcanium => null;

		public abstract ItemTiers ArmourTier { get; }


		public sealed override void UpdateArmorSet(Player player)
		{
			if (Set.HasValue)
			{
				player.setBonus = "\n" + Set.Value.Tooptip;
				ArmorSetEffects(player);
			}
		}

		public short GetArmourSizeStat()
		{
			short val = 0;
			if (Imbue is not null)
				val += (short)(Imbue.ArmourStats.Value.Corrected(Imbue).Size * (int)ArmourTier);
			return val;
		}

		public short GetArmourHasteStat()
		{
			short val = 0;
			if (Imbue is not null)
				val += (short)(Imbue.ArmourStats.Value.Corrected(Imbue).Haste * (int)ArmourTier);
			return val;
		}

		public short GetArmourAgilityStat()
		{
			short val = AOAgility;
			if (Imbue is not null)
				val += (short)(Imbue.ArmourStats.Value.Corrected(Imbue).Agility * (int)ArmourTier);
			return val;
		}

		public short GetArmourPierceStat()
		{
			short val = AOPierce;
			if (Imbue is not null)
				val += (short)(Imbue.ArmourStats.Value.Corrected(Imbue).Pierce * (int)ArmourTier);
			return val;
		}

		public short GetArmourPowerStat()
		{
			short val = AOPower;
			if (Imbue is not null)
				val += (short)(Imbue.ArmourStats.Value.Corrected(Imbue).Power * (int)ArmourTier);
			return val;
		}

		public short GetArmourAttkSpeedStat()
		{
			short val = AOAttkSpd;
			if (Imbue is not null)
				val += (short)(Imbue.ArmourStats.Value.Corrected(Imbue).Attkspeed * (int)ArmourTier);
			return val;
		}

		public sealed override bool IsArmorSet(Item head, Item body, Item legs)
		{
			if (Set.HasValue && head.ModItem is not null && head.ModItem.Mod.Name == Mod.Name && legs.ModItem is not null && legs.ModItem.Mod.Name == Mod.Name)
			{
				return head.ModItem.Name == Set.Value.OtherItems[0] && legs.ModItem.Name == Set.Value.OtherItems[1];
			}
			return false;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.defense = AODefense.FromAODefense();
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);
			if (tooltips.Contains(tooltips.Find(e => e.Name == "Social" && e.Mod == "Terraria")))
				return;

			if (MaxMana > 0)
			{
				tooltips.AddTooltip(new(Mod, "AOMaxMana", ArcaneOdysseyMod.Instance.CustomLocalization("ArmourAutoTooltip.Mana", MaxMana).Value));
			}
			if (MinionSlots > 0)
			{
				tooltips.AddTooltip(new(Mod, "MinionSlots", ArcaneOdysseyMod.Instance.CustomLocalization("ArmourAutoTooltip.Minions", MinionSlots).Value));
			}
			if (GetArmourAgilityStat() != 0)
			{
				tooltips.AddTooltip(new(Mod, "AOAgility", ArcaneOdysseyMod.Instance.CustomLocalization("ArmourAutoTooltip.Agility", Math.Round(GetArmourAgilityStat() / 5f)).Value));
				if (Main.LocalPlayer.HasTypeInInventory<TesterGoggles>())
					tooltips.AddTooltip(new(Mod, "DebugAgility", nameof(AOAgility) + " " + AOAgility));
			}
			if (GetArmourSizeStat() != 0)
			{
				tooltips.AddTooltip(new(Mod, "Size", ArcaneOdysseyMod.Instance.CustomLocalization("ArmourAutoTooltip.Size", Math.Round(GetArmourSizeStat() / SizeDivision)).Value));
				if (Main.LocalPlayer.HasTypeInInventory<TesterGoggles>())
					tooltips.AddTooltip(new(Mod, "DebugSize", nameof(Size) + " " + Size));
			}
			if (GetArmourPowerStat() != 0)
			{
				tooltips.AddTooltip(new(Mod, "AOPower", ArcaneOdysseyMod.Instance.CustomLocalization("ArmourAutoTooltip.Power", GetArmourPowerStat(), (GetArmourPowerStat() / 4f).Round()).Value));
				if (Main.LocalPlayer.HasTypeInInventory<TesterGoggles>())
					tooltips.AddTooltip(new(Mod, "DebugPower", nameof(AOPower) + " " + AOPower));
			}
			if (GetArmourAttkSpeedStat() != 0)
			{
				tooltips.AddTooltip(new(Mod, "AOAttkSpd", ArcaneOdysseyMod.Instance.CustomLocalization("ArmourAutoTooltip.Speed", Math.Round(GetArmourAttkSpeedStat() / 2.75f)).Value));
				if (Main.LocalPlayer.HasTypeInInventory<TesterGoggles>())
					tooltips.AddTooltip(new(Mod, "DebugSpeed", nameof(AOAttkSpd) + " " + AOAttkSpd));
			}
			if (GetArmourPierceStat() != 0)
			{
				tooltips.AddTooltip(new(Mod, "AOPierce", ArcaneOdysseyMod.Instance.CustomLocalization("ArmourAutoTooltip.Pierce", GetArmourPierceStat() / 5).Value));
				if (Main.LocalPlayer.HasTypeInInventory<TesterGoggles>())
					tooltips.AddTooltip(new(Mod, "DebugPierce", nameof(AOPierce) + " " + AOPierce));
			}
			if (GetArmourHasteStat() != 0)
			{
				tooltips.AddTooltip(new(Mod, "Haste", ArcaneOdysseyMod.Instance.CustomLocalization("ArmourAutoTooltip.Haste", Math.Round(GetArmourHasteStat() / HasteDivision)).Value));
				if (Main.LocalPlayer.HasTypeInInventory<TesterGoggles>())
					tooltips.AddTooltip(new(Mod, "DebugHaste", nameof(Haste) + " " + Haste));
			}
		}

		public override void UpdateEquip(Player player)
		{
			if (Arcanium.HasValue && player.TryGetImbue(out Imbuable imbue) && imbue.ArmourStats.HasValue)
			{
				if ((Arcanium.Value && imbue is MagicType) || ((!Arcanium.Value) && imbue is FightingStyle))
				{
					Item.defense = AODefense.FromAODefense() + imbue.ArmourStats.Value.Corrected(imbue).Defence.FromAODefense();
				}
			}
			player.moveSpeed += GetArmourAgilityStat() / 50f;
			player.GetDamage(DamageClass.Generic) += GetArmourPowerStat() / 100f;
			player.GetCritChance(DamageClass.Generic) += (GetArmourPowerStat() / 4f).Round();
			player.ArcaneOdyssey().StatSize += GetArmourSizeStat();
			player.ArcaneOdyssey().StatHaste += GetArmourHasteStat();
			player.GetArmorPenetration(DamageClass.Generic) += GetArmourPierceStat() / 5;
			player.GetAttackSpeed(DamageClass.Generic) += GetArmourAttkSpeedStat() / 275f;
			player.maxMinions += MinionSlots;
			player.statManaMax2 += MaxMana;
		}

		public override void SetStaticDefaults()
		{
			_ = Set?.Tooptip;
			base.SetStaticDefaults();

			ArcaneOdysseyMod.Sets.SizeStats[Type] = Size;
			ArcaneOdysseyMod.Sets.HasteStats[Type] = Haste;
		}
	}
}