using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Blocks.Tiles.Bronze;
using ArcaneOdyssey.Items.Blocks.Walls.UnsafeBronze;
using ArcaneOdyssey.Walls.Bronze;
using Terraria.GameContent.Creative;

namespace ArcaneOdyssey.Items.Blocks.Walls.Bronze
{
	public class BronzeSlabWallItem : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Common;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 400;
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<UnsafeBronzeSlabWallItem>();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DefaultToPlaceableWall(ModContent.WallType<BronzeSlabWall>());
		}

		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient<BronzeSlab>().AddTile(TileID.WorkBenches).Register();
		}
	}
}
