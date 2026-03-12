using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Blocks
{
	public class TuckerGraveItem : AOBaseItem
	{
		public override AORarities AORarity => AORarities.Special;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.Gravestone;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DefaultToPlaceableTile(ModContent.TileType<TuckerGrave>());
			Item.width = Item.height = 32;
		}
	}
}
