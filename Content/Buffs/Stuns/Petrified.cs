using ArcaneOdyssey.Content.Buffs.Base;
using System.Collections.Generic;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Buffs.Stuns
{
	public class Petrified : Stun
	{
		public override List<int> Counterparts => [BuffID.Stoned];
	}
}
