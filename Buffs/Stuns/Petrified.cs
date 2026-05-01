using ArcaneOdyssey.Buffs.Base;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Buffs.Stuns
{
	public class Petrified : Stun
	{
		public override List<int> Counterparts => [BuffID.Stoned];

		public override void Update(Player player, ref int buffIndex)
		{
			player.stoned = true;
		}
	}
}
