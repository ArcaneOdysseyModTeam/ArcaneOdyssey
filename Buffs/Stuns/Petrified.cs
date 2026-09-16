using ArcaneOdyssey.Buffs.Base;
using System.Collections.Generic;

namespace ArcaneOdyssey.Buffs.Stuns
{
	public class Petrified : Stun
	{
		public override List<int> Counterparts => [BuffID.Stoned];

		public override void Update(Player player, ref int buffIndex)
		{
			player.SetCCed();
		}
	}
}
