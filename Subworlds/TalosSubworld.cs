using ArcaneOdyssey.Biomes;
using ArcaneOdyssey.MainMenu;
using ArcaneOdyssey.NPCs.Bosses;
using ArcaneOdyssey.Subworlds.Base;
using ArcaneOdysseyMusic;

namespace ArcaneOdyssey.Subworlds
{
	public class TalosSubworld : StructureHelperSubworld
	{
		public override string StructureName => "Talos";

		public override byte OrderNum => 0; // may change later

		public override bool MetConditions => DownedBosses.DownedElius;

		public override ModBiome Biome => ModContent.GetInstance<TalosBiome>();

		public override Point SpawnLocation => new(114, 74);

		public override ArcaneOdysseyMainMenu.MainMenuStyle? MenuStyle => new(MusicTrack.Argos, Color.Transparent, "Argos", GlobalData.IsDefeated<GeneralArgos>); // change to talos theme
	}
}
