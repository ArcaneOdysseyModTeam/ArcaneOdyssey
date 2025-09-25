using ArcaneOdyssey.Content.Buffs.Base;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Buffs.MagicMarks
{
    public class BlindedEffect : AODebuff
    {
        private int stack = 1;
        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.HasBuff(Type))
            {
                stack = GetAOBuffStack(npc, buffIndex); // stacks disappear over time
                switch (stack)
                {
                    case 1:
                        return;
                    case 2:
                        return;
                    case 3:
                        return;
                    case 4:
                        return;
                    default:
                        npc.AddBuff(BuffID.Confused, 5);
                        break;
                }
            }
            if (npc.HasBuff(BuffID.Confused)) {
                npc.DelBuff(buffIndex);
				buffIndex--;
            }
        }

		public override bool ReApply(NPC npc, int time, int buffIndex)
        {
			if (npc.HasBuff(Type))
			{
				npc.buffTime[buffIndex] = (stack < 5 ? stack+1 : 5) * 5 * 60; // adds a "stack", or 5 second duration... could use "time", but other mods that change debuff duration might mess that up or something
				return true;
			}
			else return false;
		}
    }
}
