using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Blocks
{
	public class BronzeBrick : AOBaseItem
	{
		public override AORarities AORarity => AORarities.Common;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.createTile = ModContent.TileType<BronzeBrickTile>();
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
			CreateRecipe(20).AddIngredient<BronzeBar>().AddIngredient(ItemID.StoneBlock, 20).AddTile(TileID.Furnaces).Register();
			CreateRecipe().AddIngredient<BronzeBrickWallItem>(4).AddTile(TileID.WorkBenches).Register();
		}
	}
}
