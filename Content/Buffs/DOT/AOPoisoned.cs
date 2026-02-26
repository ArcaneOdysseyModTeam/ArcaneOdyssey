using ArcaneOdyssey.Content.Buffs.Base;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Buffs.DOT
{
	public class AOPoisoned : VanillaClone
	{
		public override int VanillaID => BuffID.Poisoned;
		public override List<int> Counterparts => [.. base.Counterparts, ModContent.BuffType<ElectrifiedToxins>()];
	}
}
