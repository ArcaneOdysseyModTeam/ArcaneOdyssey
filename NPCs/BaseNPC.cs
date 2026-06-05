using ArcaneOdysseyMusic;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.NPCs
{
	public abstract class BaseNPC : ModNPC, ILocalizedModType
	{
		public sealed override string LocalizationCategory => GetType().Namespace.Replace($"{Mod.Name}.");

		public virtual MusicTrack Theme => null;

		public override void SetDefaults()
		{
			if (Theme is not null && !Main.dedServ)
				Music = Theme.MusicSlot;
		}
	}
}
