using ArcaneOdyssey.Biomes.Base;
using ArcaneOdyssey.Subworlds;
using ArcaneOdysseyMusic;
using SubworldLibrary;

namespace ArcaneOdyssey.Biomes
{
	public class TalosBiome : BaseBiome
	{
		public override int Music => MusicTrack.Talos.MusicSlot;

		public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

		public override bool IsBiomeActive(Player player)
		{
			if (Main.myPlayer == player.whoAmI)
			{
				return SubworldSystem.IsActive<TalosSubworld>();
			}
			return false;
		}

		public override bool MapBackgroundFullbright => true;

		public override void Load()
		{
			On_Main.UpdateAtmosphereTransparencyToSkyColor += DisableSpaceRendering;
		}

		private static void DisableSpaceRendering(On_Main.orig_UpdateAtmosphereTransparencyToSkyColor orig)
		{
			if (!Main.gameMenu && AOUtils.InAOSubworld)
				return;
			orig();
		}
	}
}
