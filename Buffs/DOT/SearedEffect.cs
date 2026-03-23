using ArcaneOdyssey.Buffs.Base;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Buffs.DOT
{
	public class SearedEffect : MagicMark
	{
		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.wet && !npc.lavaWet)
			{
				npc.DelBuff(buffIndex);
				buffIndex--;
				return;
			}
			Dust newDust = Dust.NewDustDirect(npc.position, npc.Hitbox.Width, npc.Hitbox.Height, DustID.CrimsonTorch, (0.5f - Main.rand.NextFloat()) * 2f, (0.5f - Main.rand.NextFloat()) * 2f, 1, default, 3f);
			newDust.noGravity = true;
			npc.ArcaneOdyssey().seared = true;
		}
	}
}
