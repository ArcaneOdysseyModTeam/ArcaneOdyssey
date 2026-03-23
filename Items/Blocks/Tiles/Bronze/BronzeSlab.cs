using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Blocks.Walls.Bronze;
using ArcaneOdyssey.Tiles.Bronze;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Blocks.Tiles.Bronze
{
	public class BronzeSlab : BaseItem
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
			Item.DefaultToPlaceableTile(ModContent.TileType<BronzeSlabTile>());
		}

		public override void AddRecipes()
		{
			CreateRecipe(2).AddIngredient<BronzeBrick>().AddTile(TileID.HeavyWorkBench).Register();
			CreateRecipe().AddIngredient<BronzeSlabWallItem>(4).AddTile(TileID.WorkBenches).Register();
		}
	}
}
