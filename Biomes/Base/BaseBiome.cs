namespace ArcaneOdyssey.Biomes.Base
{
	public abstract class BaseBiome : ModBiome, ILocalizedModType
	{
		public sealed override string LocalizationCategory => GetType().Namespace.Replace($"{Mod.Name}.");
	}
}
