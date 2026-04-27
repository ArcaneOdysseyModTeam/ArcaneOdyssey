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
	public class BronzeBrickWallItem : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Common;

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
