using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ArcaneOdyssey.Content.Tiles.MusicBoxes
{
	public class TitleMusicBoxTile : ModTile
	{
		public override void EmitParticles(int i, int j, Tile tile, short tileFrameX, short tileFrameY, Color tileLight, bool visible)
		{
            WorldGen.KillTile(i, j);
		}
	}
}