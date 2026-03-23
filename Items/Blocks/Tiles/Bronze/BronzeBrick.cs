using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Blocks.Walls.Bronze;
using ArcaneOdyssey.Items.Materials;
using ArcaneOdyssey.Tiles.Bronze;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Blocks.Tiles.Bronze
{
	public class BronzeBrick : BaseItem
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
			Item.DefaultToPlaceableTile(ModContent.TileType<BronzeBrickTile>());
		}

		public override void AddRecipes()
		{
			CreateRecipe(20).AddIngredient<BronzeBar>().AddIngredient(ItemID.StoneBlock, 20).AddTile(TileID.Furnaces).Register();
			CreateRecipe().AddIngredient<BronzeBrickWallItem>(4).AddTile(TileID.WorkBenches).Register();
		}
	}
}
