using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Tiles.BossTrophies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.BossTrophies
{
	public class DuskTrophy : BaseItem
	{
		public override AORarities AORarity => AORarities.Common;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DefaultToPlaceableTile(ModContent.TileType<DuskTrophyTile>());
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(gold: 1);
			Item.width = Item.height = 32;
		}
	}
}
