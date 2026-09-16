using System.Collections.Generic;

namespace ArcaneOdyssey.Buffs.Base
{
	public abstract class BaseBuff : ModBuff, ILocalizedModType
	{
		public sealed override string LocalizationCategory => GetType().Namespace.Replace($"{Mod.Name}.");

		public virtual List<int> Counterparts => [];

		public override void SetStaticDefaults()
		{
			BuffID.Sets.GrantImmunityWith[Type] = Counterparts;
		}
	}
}
