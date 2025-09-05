using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Tiles.MusicBoxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace ArcaneOdyssey.Content.Items.Equipment.MusicBoxes
{
    public class DarkSeaMusicBox : MusicBox
    {
        public override int MusicBoxTile => ModContent.TileType<DarkSeaMusicBoxTile>();
        public override string MusicFilePath => "Music/DarkSea";
    }
}
