using ArcaneOdysseyMusic;
using Terraria.ModLoader;

namespace ArcaneOdyssey.NPCs
{
	public abstract class AOBaseNPC : ModNPC, ILocalizedModType
	{
		public override string LocalizationCategory => GetType().Namespace.Replace($"{Mod.Name}.");

		public virtual AOMusicTrack Theme => null;

		public override void SetDefaults()
		{
			if (Theme is not null)
				Music = Theme.MusicSlot;
		}
	}
}
