using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Materials
{
	public class BronzeBar : AOBaseItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 25;
		}

		public override AORarities AORarity => AORarities.Uncommon;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = 30;
			Item.height = 24;
			Item.createTile = ModContent.TileType<BronzeBarTile>();
			Item.maxStack = 9999;
			Item.value = Item.sellPrice(silver: 30); // bit less than hellstone
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
		}

		public override void AddRecipes()
		{
			RecipeGroup coppergroup = new(() => Language.GetTextValue("LegacyMisc.37") + new Item(ItemID.CopperOre).Name, ItemID.CopperOre, ItemID.TinOre);
			RecipeGroup.RegisterGroup("AnyCopperOre", coppergroup);
			RecipeGroup goldgroup = new(() => Language.GetTextValue("LegacyMisc.37") + new Item(ItemID.GoldOre).Name, ItemID.GoldOre, ItemID.PlatinumOre);
			RecipeGroup.RegisterGroup("AnyGoldOre", goldgroup);
			CreateRecipe(10).AddRecipeGroup(coppergroup, 4).AddRecipeGroup(goldgroup, 4).
				AddIngredient(ItemID.TissueSample, 1).AddTile(TileID.Furnaces).Register();
		}
	}
}