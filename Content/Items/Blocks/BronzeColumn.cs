using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Tiles.Bronze;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Blocks
{
	public class BronzeColumn : AOBaseItem
	{
		public override AORarities AORarity => AORarities.Common;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DefaultToPlaceableTile(ModContent.TileType<BronzeColumnTile>());
		}

		public override void AddRecipes()
		{
			CreateRecipe(2).AddIngredient<BronzeBrick>().AddTile(TileID.Sawmill).Register();
			CreateRecipe().AddIngredient<BronzeColumnWallItem>(4).AddTile(TileID.WorkBenches).Register();
		}
	}
}
