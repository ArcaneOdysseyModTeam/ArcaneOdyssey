using ArcaneOdyssey.Buffs.Base;
using System.Collections.Generic;

namespace ArcaneOdyssey.Buffs.Stuns
{
	public class Tangled : Stun
	{
		public const int VanillaID = BuffID.Webbed;

		public override string Texture => $"Terraria/Images/Buff_{VanillaID}";

		public override List<int> Counterparts => [VanillaID];

		public override LocalizedText Description => Language.GetText($"BuffDescription.{BuffID.Search.GetName(VanillaID)}");

		public override LocalizedText DisplayName => Language.GetText($"BuffName.{BuffID.Search.GetName(VanillaID)}");

		private int stack = 1;

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
			npc.buffTime[buffIndex] += time;
			return true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.webbed = true;
		}
	}
}
