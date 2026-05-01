using ArcaneOdyssey.Buffs.Base;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Buffs.DOT
{
	public class Melting : MagicMark
	{
		public override List<int> Counterparts => [BuffID.OnFire3, BuffID.Burning];

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.wet && !npc.lavaWet)
			{
				npc.DelBuff(buffIndex);
				buffIndex--;
				return;
			}
			npc.ArcaneOdyssey().melting = true;
			if (!Main.dedServ)
			{
				var dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Lava);
				dust.velocity *= 0.4f;
			}
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ExternalModSupport.RegisterDoT(Type);
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.burned = true;
		}
	}
}
