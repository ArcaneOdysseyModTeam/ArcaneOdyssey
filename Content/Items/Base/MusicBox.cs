using System;
using Terraria.ModLoader;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Items.Base
{
    public abstract class MusicBox : ModItem
    {
        public abstract int MusicBoxTile { get; }

        public override void SetStaticDefaults()
        {
            ItemID.Sets.CanGetPrefixes[Type] = false;
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.MusicBox;
        }

        public override void SetDefaults()
        {
            Item.DefaultToMusicBox(MusicBoxTile, 0);
        }
    }
}