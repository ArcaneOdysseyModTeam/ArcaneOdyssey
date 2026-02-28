using ArcaneOdyssey.Content.Buffs.Base;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Buffs.DOT
{
	public class Melting : AODebuff
	{
		public override List<int> Counterparts => [BuffID.OnFire3, BuffID.Burning];

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.ArcaneOdyssey().melting = true;
			if (!Main.dedServ)
			{
				var dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Lava);
				dust.velocity *= 0.4f;
			}
		}
	}
}
