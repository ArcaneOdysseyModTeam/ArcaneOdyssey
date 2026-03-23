using ArcaneOdyssey.Buffs.Base;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Buffs.DOT
{
	public class Scalding : MagicMark
	{
		public override void Update(NPC npc, ref int buffIndex)
		{
			if (!Main.dedServ)
			{
				var dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.SteampunkSteam);
				dust.velocity *= 0.4f;
			}
			npc.ArcaneOdyssey().scalding = true;
		}

		public override List<int> Counterparts => [BuffID.Frostburn, BuffID.Frostburn2];
	}
}
