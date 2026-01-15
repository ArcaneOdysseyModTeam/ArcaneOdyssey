using ArcaneOdyssey.Content.Buffs.Base;
using Terraria;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Buffs.DOT
{
	public class Singed : AODebuff
	{
		private int stack = 1;

		public override string Texture => Mod.Name + "/Assets/Debuff";

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.HasBuff(Type))
			{
				stack = GetAOBuffStack(npc, buffIndex); // stacks disappear over time
				npc.ArcaneOdyssey().singedstacks = stack;
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
