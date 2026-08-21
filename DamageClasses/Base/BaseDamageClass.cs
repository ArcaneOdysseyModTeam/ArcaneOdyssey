namespace ArcaneOdyssey.DamageClasses.Base
{
	public abstract class BaseDamageClass : DamageClass, ILocalizedModType
	{
		public sealed override string LocalizationCategory => GetType().Namespace.Replace($"{Mod.Name}.");
	}
}
