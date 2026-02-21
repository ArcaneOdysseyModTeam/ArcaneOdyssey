using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Tiles.Bronze;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Blocks
{
	public class BronzeSlab : AOBaseItem
	{
		public override AORarities AORarity => AORarities.Common;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DefaultToPlaceableTile(ModContent.TileType<BronzeSlabTile>());
		}

		public override void AddRecipes()
		{
			CreateRecipe(2).AddIngredient<BronzeBrick>().AddTile(TileID.HeavyWorkBench).Register();
			CreateRecipe().AddIngredient<BronzeSlabWallItem>(4).AddTile(TileID.WorkBenches).Register();
		}
	}
}
