using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Base
{
	public abstract class AOArmour : ModItem
	{
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
		/// At max item level btw
		/// </summary>
		public virtual int AOValue => 0;

		/// <summary>
		/// At max item level btw
		/// </summary>
		public virtual int AOSize => 0;

		/// <summary>
		/// At max item level btw
		/// </summary>
		public virtual int AOPower => 0;

		/// <summary>
		/// Without enchantments ect
		/// </summary>
		public virtual int AORarity => AORarities.Common;

		public virtual int MinionSlots => 0;

		public virtual int MaxMana => 0;

		public virtual void SetDefaultsArmour()
		{

		}

		public override void SetDefaults()
		{
			Item.defense = AODefense.FromAODefense();
			SetDefaultsArmour();
			Item.value = GalleonToCopper(AOValue, AORarity);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (MaxMana > 0) 
			{
				tooltips.Add(new(Mod, "MaxMana", Mod.CustomLocalization("ArmourAutoTooltip.Mana", [MaxMana]).Value));
			}
			if (MinionSlots > 0)
			{
				tooltips.Add(new(Mod, "MinionSlots", Mod.CustomLocalization("ArmourAutoTooltip.Minions", [MinionSlots]).Value));
			}
			if (AOAgility > 0)
			{
				tooltips.Add(new(Mod, "MoveSpeed", Mod.CustomLocalization("ArmourAutoTooltip.Speed", [AOAgility/300f]).Value));
			}
			if (AOSize > 0)
			{
				tooltips.Add(new(Mod, "AttackSize", Mod.CustomLocalization("ArmourAutoTooltip.Size", [AOSize/5f]).Value));
			}
			if (AOPower > 0)
			{
				tooltips.Add(new(Mod, "BaseDmg", Mod.CustomLocalization("ArmourAutoTooltip.Power", [AOPower]).Value));
			}
		}

		public virtual void UpdateArmour(Player player) {}

		public override void UpdateEquip(Player player)
		{
			player.GetAttackSpeed(DamageClass.Generic) += AOAttkSpd/500;
			player.GetDamage(DamageClass.Generic).Base += AOPower;
			player.moveSpeed += AOAgility / 300f;
			player.AOPlayer().AOSizeStat += AOSize;
			player.maxMinions += MinionSlots;
			player.statManaMax2 += MaxMana;
			UpdateArmour(player);
		}
	}
}