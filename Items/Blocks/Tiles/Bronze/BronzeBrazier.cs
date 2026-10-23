using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Tiles.Bronze;

namespace ArcaneOdyssey.Items.Blocks.Tiles.Bronze
{
	public class BronzeBrazier : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Uncommon;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DefaultToPlaceableTile(ModContent.TileType<BronzeBrazierTile>());
			Item.value = Item.sellPrice(gold: 2);
			Item.width = 60;
			Item.height = 28;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BronzeBrick>(4).AddIngredient(ItemID.Torch, 4).AddTile(TileID.Anvils).Register();
		}
	}
}
