using ArcaneOdyssey.Buffs.Base;

namespace ArcaneOdyssey.Buffs.MagicMarks
{
	public class CharredEffect : MagicMark
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
				var dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Ash);
				dust.velocity *= 0.1f;
				dust.noGravity = true;
			}
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.wet && !player.lavaWet)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
				return;
			}
			if (!Main.dedServ)
			{
				var dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.Ash);
				dust.velocity *= 0.1f;
				dust.noGravity = true;
			}
		}
	}
}
