using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Materials
{
	public class BronzeBar : AOBaseItem
	{
		public override ItemType ItemType => ItemType.Material;
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 15;
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 24;
			Item.createTile = ModContent.TileType<BronzeBarTile>();
			Item.maxStack = 9999;
			Item.value = Item.sellPrice(silver: 30); // bit less than hellstone
			Item.rare = ItemRarityID.Green;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
		}

		public override void AddRecipes() // only exists in debug mode :)
		{
			CreateRecipe().
				AddIngredient(ItemID.CopperOre, 4).
				AddIngredient(ItemID.TinOre, 4).
				AddTile(TileID.Hellforge).
				Register();
		}
	}
}