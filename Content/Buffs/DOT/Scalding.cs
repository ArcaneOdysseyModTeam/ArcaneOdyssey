using ArcaneOdyssey.Content.Buffs.Base;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Buffs.DOT
{
	public class Scalding : AODebuff
	{
		public override void Update(NPC npc, ref int buffIndex)
		{
			if (!Main.dedServ)
			{
				var dust = Dust.NewDustDirect(npc.Center, 0, 0, DustID.SteampunkSteam, 0f, 0f, 1, default, 1f);
				dust.velocity *= 0.4f;
			}
			npc.ArcaneOdyssey().scalding = true;
		}

		public override List<int> Counterparts => [BuffID.Frostburn, BuffID.Frostburn2];
	}
}
