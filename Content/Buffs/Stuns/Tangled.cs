using ArcaneOdyssey.Content.Buffs.Base;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Buffs.Stuns
{
	public class Tangled : Stun
	{
		public override string Texture => $"Terraria/Images/Buff_{BuffID.Webbed}";
		private int stack = 1;

		public override int[] Counterparts => [BuffID.Webbed];

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.HasBuff(Type))
			{
				stack = AOUtils.GetAOBuffStack(npc, buffIndex); // stacks disappear over time
				switch (stack)
				{
					case 1:
						return;
					case 2:
						return;
					case 3:
						return;
					case 4:
						if (!npc.boss && npc.ArcaneOdyssey().StunCD <= 0 || LiterallyCheating)
						{
							npc.ArcaneOdyssey().AOStunned = true;
						}
						break;
					default: // if the stack number isnt valid or over 4, just delete the buff
						npc.DelBuff(buffIndex);
						buffIndex--;
						break;
				}
			}
		}

		public override bool ReApply(NPC npc, int time, int buffIndex)
		{
			if (npc.HasBuff(Type))
			{
				npc.buffTime[buffIndex] += time;
				return true;
			}
			return false;
		}
	}
}
