using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Base
{
	public abstract class FightingStyle : Imbuable, ILocalizedModType
	{
		public override string LocalizationCategory => "FightingStyles." + ImbuableTier;
	}
}
