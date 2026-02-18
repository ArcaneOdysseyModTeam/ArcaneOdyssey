using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Blocks
{
	public class BronzeColumn : AOBaseItem
	{
		public override AORarities AORarity => AORarities.Common;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.createTile = ModContent.TileType<BronzeColumnTile>();
			Item.width = Item.height = 16;
			Item.maxStack = Item.CommonMaxStack;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(2).AddIngredient<BronzeBrick>().AddTile(TileID.Sawmill).Register();
			CreateRecipe().AddIngredient<BronzeColumnWallItem>(4).AddTile(TileID.WorkBenches).Register();
		}
	}
}
