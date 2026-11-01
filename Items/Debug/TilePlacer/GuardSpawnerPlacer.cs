using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Tiles.Subworlds;

namespace ArcaneOdyssey.Items.Debug.TilePlacer
{
	public class GuardSpawnerPlacer : DevTilePlacer
	{
		public override int Tile => ModContent.TileType<TalosGuardSpawner>();
	}
}
