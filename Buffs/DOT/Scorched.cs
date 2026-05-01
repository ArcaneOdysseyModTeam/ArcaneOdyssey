using ArcaneOdyssey.Buffs.Base;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Buffs.DOT
{
	public class Scorched : MagicMark
	{
		public override List<int> Counterparts => [BuffID.ShadowFlame, ModContent.BuffType<Shadowflame>()];

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.wet && !npc.lavaWet)
			{
				npc.DelBuff(buffIndex);
				buffIndex--;
				return;
			}
			npc.ArcaneOdyssey().scorched = true;
			if (!Main.dedServ)
			{
				var dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Shadowflame);
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
			player.onFire3 = true;
		}
	}
}
