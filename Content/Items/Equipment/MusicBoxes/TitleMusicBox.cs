using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Tiles.MusicBoxes;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Equipment.MusicBoxes
{
    public class TitleMusicBox : MusicBox
    {
        public override int MusicBoxTile => ModContent.TileType<TitleMusicBoxTile>();
    }
}
