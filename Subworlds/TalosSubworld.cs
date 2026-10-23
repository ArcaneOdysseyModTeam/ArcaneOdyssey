using ArcaneOdyssey.Biomes;
using ArcaneOdyssey.Subworlds.Base;

namespace ArcaneOdyssey.Subworlds
{
	public class TalosSubworld : StructureHelperSubworld
	{
		public override string StructureName => "FortTalos";

		public override ushort OrderNum => 0; // may change later

		public override bool MetConditions => DownedBosses.DownedElius;

		public override ModBiome Biome => ModContent.GetInstance<TalosBiome>();

		public override Point SpawnLocation => new(114, 74);
	}
}
