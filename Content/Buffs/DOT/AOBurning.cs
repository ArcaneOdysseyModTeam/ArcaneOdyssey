using ArcaneOdyssey.Content.Buffs.Base;
using System.Collections.Generic;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Buffs.DOT
{
	public class AOBurning : VanillaClone
	{
		public override int VanillaID => BuffID.OnFire;
		public override List<int> Counterparts => [BuffID.OnFire];
	}
}
