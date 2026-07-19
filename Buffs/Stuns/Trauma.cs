using ArcaneOdyssey.Buffs.Base;
using System.Collections.Generic;

namespace ArcaneOdyssey.Buffs.Stuns
{
	public class Trauma : Stun
	{
		public override bool LiterallyCheating => true;
		public override List<int> Counterparts => [BuffID.Horrified, BuffID.MoonLeech];
	}
}
