using ArcaneOdyssey.Buffs.Base;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Buffs.DOT
{
	public class Corroding : AODebuff
	{
		public override List<int> Counterparts => [BuffID.Venom];

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.ArcaneOdyssey().corroding = true;
			if (!Main.dedServ)
			{
				var dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Water_Corruption);
				dust.velocity *= 0.4f;
			}
		}
	}
}
