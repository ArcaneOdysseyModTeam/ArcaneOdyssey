using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Tiles.MusicBoxes
{
	public class TitleMusicBoxTile : ModTile
	{
		public override void EmitParticles(int i, int j, Tile tile, short tileFrameX, short tileFrameY, Color tileLight, bool visible)
		{
			WorldGen.KillTile(i, j);
		}
	}
}