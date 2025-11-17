using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Tiles.BossTrophies;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArcaneOdyssey.Content.Items.BossTrophies
{
    public class EvanderTrophy : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<EvanderTrophyTile>());
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(gold: 1);
            Item.width = Item.height = 32;
        }
    }
}
