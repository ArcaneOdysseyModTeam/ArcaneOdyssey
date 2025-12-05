using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using ArcaneOdyssey.Content.Tiles.BossTrophies;

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
