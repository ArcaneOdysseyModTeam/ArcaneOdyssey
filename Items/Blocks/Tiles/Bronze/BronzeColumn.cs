using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Blocks.Walls.Bronze;
using ArcaneOdyssey.Tiles.Bronze;
using Terraria.GameContent.Creative;

namespace ArcaneOdyssey.Items.Blocks.Tiles.Bronze
{
	public class BronzeColumn : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Common;

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
