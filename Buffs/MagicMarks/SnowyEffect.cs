using ArcaneOdyssey.Buffs.Base;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Buffs.MagicMarks
{
	public class SnowyEffect : AODebuff
	{
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
				var dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.SnowBlock);
				dust.velocity *= 0.1f;
				dust.noGravity = true;
			}
		}
	}
}
