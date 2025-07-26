using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Buffs.MagicMarks
{
    public class CrystalStackIIII : MagicMark {
         public override void Update(NPC npc, ref int buffIndex) {
            //add dust later
            for(int i = 0;i<NPC.maxBuffs;i++) {
                if(npc.buffType[i] == ModContent.BuffType<CrystalStackI>() || npc.buffType[i] == ModContent.BuffType<CrystalStackII>() || npc.buffType[i] == ModContent.BuffType<CrystalStackIII>() || npc.buffType[i] == ModContent.BuffType<CrystalStackIIII>()){
                    npc.DelBuff(i);
                    i--;
                }
            }
         }
    }
}
