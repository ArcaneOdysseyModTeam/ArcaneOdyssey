using ArcaneOdyssey.Content.Buffs.Base;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Buffs.Stuns
{
	public class Trauma : Stun
	{
		public override bool LiterallyCheating => true;
		public override int[] Counterparts => [BuffID.Horrified, BuffID.MoonLeech];
	}
}
