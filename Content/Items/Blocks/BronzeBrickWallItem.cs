using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Walls;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Blocks
{
	public class BronzeBrickWallItem : AOBaseItem
	{
		public override AORarities AORarity => AORarities.Common;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.createWall = ModContent.WallType<BronzeBrickWall>();
			Item.width = Item.height = 24;
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
			CreateRecipe(4).AddIngredient<BronzeBrick>().AddTile(TileID.WorkBenches).Register();
		}
	}
}
