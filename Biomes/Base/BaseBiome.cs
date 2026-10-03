namespace ArcaneOdyssey.Biomes.Base
{
	public abstract class BaseBiome : ModBiome, ILocalizedModType
	{
		public sealed override string LocalizationCategory => GetType().Namespace.Replace($"{Mod.Name}.");

		public virtual string Texture => (GetType().Namespace + "." + Name).Replace('.', '/');

		public override string BestiaryIcon => Texture + "_Icon";
		public override string BackgroundPath => Texture + "_Background";
		public override string MapBackground => BackgroundPath;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
		}
	}
}
