using Terraria.ModLoader;

namespace ArcaneOdyssey.NPCs
{
	public abstract class AOBaseNPC : ModNPC, ILocalizedModType
	{
		public override string LocalizationCategory => GetType().Namespace.Replace($"{Mod.Name}.");
	}
}
