using ArcaneOdyssey.Content.Buffs.Base;
using System.Collections.Generic;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Buffs.Stuns
{
	public class Trauma : Stun
	{
		public override bool LiterallyCheating => true;
		public override List<int> Counterparts => [BuffID.Horrified, BuffID.MoonLeech];
	}
}
