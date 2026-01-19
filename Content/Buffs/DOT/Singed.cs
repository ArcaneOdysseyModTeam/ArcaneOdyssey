using ArcaneOdyssey.Content.Buffs.Base;
using Terraria;
using static ArcaneOdyssey.AOUtils;
using Terraria.ID;
using System;

namespace ArcaneOdyssey.Content.Buffs.DOT
{
	public class Singed : AODebuff
	{
		private int stack = 1;

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.HasBuff(Type))
			{
				stack = Math.Clamp(GetAOBuffStack(npc, buffIndex), 0, 20); // stacks disappear over time
				npc.ArcaneOdyssey().singedstacks = stack;
			}
			if (!Main.dedServ)
			{
				Dust.NewDustDirect(npc.position, npc.Hitbox.Width, npc.Hitbox.Height, DustID.CrimsonTorch, (0.5f - Main.rand.NextFloat()) * 2f, (0.5f - Main.rand.NextFloat()) * 2f, 1, default, 3f);
			}
		}

		public override bool ReApply(NPC npc, int time, int buffIndex)
		{
			if (npc.HasBuff(Type))
			{
				npc.buffTime[buffIndex] += time;
				return true;
			}
			else return false;
		}
	}
}
