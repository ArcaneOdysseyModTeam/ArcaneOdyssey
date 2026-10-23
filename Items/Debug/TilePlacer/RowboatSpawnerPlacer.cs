using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Tiles.Subworlds;

namespace ArcaneOdyssey.Items.Debug.TilePlacer
{
	public class RowboatSpawnerPlacer : DevTilePlacer
	{
		public override int Tile => ModContent.TileType<RowboatSpawnerTile>();
	}
}
