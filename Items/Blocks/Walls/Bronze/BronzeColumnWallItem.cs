using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Blocks.Tiles.Bronze;
using ArcaneOdyssey.Items.Blocks.Walls.UnsafeBronze;
using ArcaneOdyssey.Walls.Bronze;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Blocks.Walls.Bronze
{
	public class BronzeColumnWallItem : BaseItem
	{
		public override AORarities AORarity => AORarities.Common;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 400;
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<UnsafeBronzeColumnWallItem>();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DefaultToPlaceableWall(ModContent.WallType<BronzeColumnWall>());
		}

		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient<BronzeColumn>().AddTile(TileID.WorkBenches).Register();
		}
	}
}
