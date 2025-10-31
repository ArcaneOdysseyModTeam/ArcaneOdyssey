using ArcaneOdyssey.Content.Buffs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace ArcaneOdyssey.Content.Buffs.DOT
{
    public class ElectrifiedToxins : AODebuff
    {
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.ArcaneOdyssey().ElecToxins = true;
        }
    }
}
