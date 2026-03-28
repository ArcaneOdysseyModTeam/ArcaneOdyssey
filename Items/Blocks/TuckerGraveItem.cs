using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Blocks
{
	public class TuckerGraveItem : BaseItem
	{
		public override Rarities Rarity => Rarities.Common;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.Gravestone;
			if (ItemID.Sets.ShimmerTransformToItem[ItemID.Gravestone] == -1)
			{
				ItemID.Sets.ShimmerTransformToItem[ItemID.Gravestone] = Type;
			}
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DefaultToPlaceableTile(ModContent.TileType<TuckerGrave>());
			Item.width = Item.height = 32;
		}
	}
}
