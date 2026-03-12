using ArcaneOdyssey.Buffs.Base;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Buffs.MagicMarks
{
	public class FreezingEffect : AODebuff
	{
		public override List<int> Counterparts => [BuffID.Chilled];
		public override string Texture => $"Terraria/Images/Buff_{BuffID.Chilled}";
		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.wet && !npc.lavaWet)
			{
				npc.DelBuff(buffIndex);
				buffIndex--;
				return;
			}
			if (!Main.dedServ)
			{
				var dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.SnowflakeIce);
				dust.velocity *= 0.1f;
				dust.noGravity = true;
			}
		}
	}
}
