using Terraria.ModLoader;

namespace ArcaneOdyssey.Prefixes
{
	public abstract class BasePrefix : ModPrefix, ILocalizedModType
	{
		public override string LocalizationCategory => GetType().Namespace.Replace(Mod.Name + '.');
	}
}
