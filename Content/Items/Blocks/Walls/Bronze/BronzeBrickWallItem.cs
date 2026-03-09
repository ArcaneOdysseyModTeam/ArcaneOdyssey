using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Blocks.Tiles.Bronze;
using ArcaneOdyssey.Content.Items.Blocks.Walls.UnsafeBronze;
using ArcaneOdyssey.Content.Walls.Bronze;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Blocks.Walls.Bronze
{
	public class BronzeBrickWallItem : AOBaseItem
	{
		public override AORarities AORarity => AORarities.Common;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 400;
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<UnsafeBronzeBrickWallItem>();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DefaultToPlaceableWall(ModContent.WallType<BronzeBrickWall>());
		}

		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient<BronzeBrick>().AddTile(TileID.WorkBenches).Register();
		}
	}
}
