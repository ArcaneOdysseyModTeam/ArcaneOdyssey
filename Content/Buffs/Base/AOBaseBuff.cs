using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Buffs.Base
{
	public abstract class AOBaseBuff : ModBuff, ILocalizedModType
	{
		public override string LocalizationCategory => GetType().Namespace.Replace($"{Mod.Name}.Content.");

		public virtual List<int> Counterparts => [];

		public override void SetStaticDefaults()
		{
			BuffID.Sets.GrantImmunityWith[Type] = Counterparts;
		}
	}
}
