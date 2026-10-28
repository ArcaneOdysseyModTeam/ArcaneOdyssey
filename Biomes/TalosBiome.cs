using ArcaneOdyssey.Biomes.Base;
using ArcaneOdyssey.Subworlds;
using ArcaneOdysseyMusic;
using SubworldLibrary;

namespace ArcaneOdyssey.Biomes
{
	public class TalosBiome : BaseBiome
	{
		public override int Music => MusicTrack.Argos.MusicSlot; // change to talos theme

		public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

		public override bool IsBiomeActive(Player player)
		{
			if (Main.myPlayer == player.whoAmI)
			{
				return SubworldSystem.IsActive<TalosSubworld>();
			}
			return false;
		}

		public override void Load()
		{
			On_Main.RenderBackground += SubworldDisableBackground;
		}

		private static void SubworldDisableBackground(On_Main.orig_RenderBackground orig, Main self)
		{
			if (AOUtils.InAOSubworld)
			{
				return;
			}
			orig(self);
		}
	}
}
