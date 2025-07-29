using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Buffs.MagicMarks
{
    public class AOBleed : MagicMark
    {
        private int frameNum = 0;
        System.Random rnd = new System.Random();
        public override void Update(NPC npc, ref int buffIndex) {
            frameNum++;
            if(frameNum>20){
                frameNum = 0;
                npc.SimpleStrikeNPC(3,0,false,0f,null,false,0f,false);
                Dust.NewDust(npc.position,1,1,DustID.Blood,(0.5f-rnd.NextSingle())*2f,(0.5f-rnd.NextSingle())*2f,1,default,1f);
            }
        }
    }
}
