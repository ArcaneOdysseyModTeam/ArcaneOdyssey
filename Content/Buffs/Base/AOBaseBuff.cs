using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Buffs.Base
{
	public abstract class AOBaseBuff : ModBuff, ILocalizedModType
	{
		public override string LocalizationCategory => GetType().Namespace.Replace($"{Mod.Name}.Content.");
	}
}
