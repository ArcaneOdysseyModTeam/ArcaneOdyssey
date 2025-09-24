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
        private int stack;
        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.HasBuff(Type))
            {
                stack = GetAOBuffStack(npc, buffIndex); // stacks disappear over time
                switch (stack)
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    default:
						npc.AddBuff(BuffID.Confused, 5, false);
						break;
                }
            }
        }

		public override bool ReApply(NPC npc, int time, int buffIndex)
        {
			if (npc.HasBuff<BlindedEffect>())
			{
				npc.buffTime[buffIndex] = (stack+1) * 5 * 60; // adds a "stack", or 5 second duration... could use "time", but other mods that change debuff duration might mess that up or something
				return true;
			}
			else return false;
		}
    }
}
