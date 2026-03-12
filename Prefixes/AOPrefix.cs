using Terraria.ModLoader;

namespace ArcaneOdyssey.Prefixes
{
	public abstract class AOPrefix : ModPrefix
	{
		public override string LocalizationCategory => GetType().Namespace.Replace($"{Mod.Name}.");
	}
}
