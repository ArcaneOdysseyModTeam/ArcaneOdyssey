using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using ArcaneOdyssey.Content.Tiles.BossTrophies;
using ArcaneOdyssey.Content.Items.Base;

namespace ArcaneOdyssey.Content.Items.BossTrophies
{
	public class EvanderTrophy : AOBaseItem
	{
		public override AORarities AORarity => AORarities.Common;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DefaultToPlaceableTile(ModContent.TileType<EvanderTrophyTile>());
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(gold: 1);
			Item.width = Item.height = 32;
		}
	}
}
