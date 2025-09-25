using System;
using Terraria.ModLoader;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Items.Base
{
    public abstract class MusicBox : AOBaseItem
    {
        public abstract int MusicBoxTile { get; }
        public abstract string MusicFilePath { get; }

		public override AOUtils.ItemType ItemType => AOUtils.ItemType.Accessory;

		public override void SetStaticDefaults()
        {
            ItemID.Sets.CanGetPrefixes[Type] = false;
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.MusicBox;

            if (!string.IsNullOrEmpty(MusicFilePath))
                MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, MusicFilePath), Type, MusicBoxTile);
        }

        public override void SetDefaults()
        {
            Item.DefaultToMusicBox(MusicBoxTile, 0);
			Item.rare = ItemRarityID.Orange;
        }
    }
}
