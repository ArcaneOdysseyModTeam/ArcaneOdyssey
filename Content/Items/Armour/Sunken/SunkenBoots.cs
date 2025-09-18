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

namespace ArcaneOdyssey.Content.Items.Armour.Sunken
{
	[AutoloadEquip(EquipType.Legs)]
	public class SunkenBoots : AOArmour
	{
		public override int AODefense => 204;
		public override int AOSize => 23;
		public override int AOAttkSpd => 23;
		public override AORarities AORarity => AORarities.Rare;

		public override int AOAgility => 30;
		public override int AOValue => 1350;

		public override void SetDefaultsArmour()
		{
			Item.width = Item.height = 38;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.ModItem is SunkenHelm && body.ModItem is SunkenChest;
		}

		public override void UpdateArmour(Player player)
		{
		}

		public override void UpdateArmorSet(Player player)
		{
			player.ArcaneOdyssey().sunkenArmour = true;
			player.setBonus = Mod.CustomLocalization($"Items.SunkenBoots.SetText").Value;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<ArcaniumScrap>(3).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
