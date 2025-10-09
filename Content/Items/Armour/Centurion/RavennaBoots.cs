using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Items.Armour.Centurion
{
	[AutoloadEquip(EquipType.Legs)]
	public class RavennaBoots : AOArmour
	{
		public override int AODefense => 56;
		public override int AOSize => AODefense / 20;
		public override int AOAttkSpd => AODefense / 20;
		public override AORarities AORarity => AORarities.Common;
		public override int AOValue => 30;

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.ModItem is RavennaHelm && body.ModItem is RavennaChest;
		}

		public override void UpdateArmorSet(Player player)
		{
			player.GetModPlayer<CenturionPlayer>().bronzeSetBonus = true;
			player.setBonus = Mod.CustomLocalization($"Items.RavennaBoots.SetText").Value;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BronzeBar>(30).AddTile(TileID.Anvils).Register();
		}
	}

	public class CenturionPlayer : ModPlayer
	{
		public bool bronzeSetBonus = false;
		public override void ResetEffects()
		{
			bronzeSetBonus = false;
		}
	}
}
