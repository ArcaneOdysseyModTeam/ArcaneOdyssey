using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Buffs.Stuns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Steamworks;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq.Expressions;
using static ArcaneOdyssey.AOUtils;
using Microsoft.Xna.Framework;
using ArcaneOdyssey.Content.Buffs.Base;

namespace ArcaneOdyssey.Content.Buffs.MagicMarks
{
    public class CrystalStackIIII : Base.AODebuff {
        System.Random rnd = new System.Random();
         public override void Update(NPC npc, ref int buffIndex) {
            //dust
            for(int dustCountInt = 0;dustCountInt<10;dustCountInt++){
                    Dust.NewDust(npc.position+ new Vector2((float)npc.width/2f,(float)npc.height/2f),1,1,DustID.GemRuby,(0.5f-rnd.NextSingle())*5f,(0.1f-rnd.NextSingle())*5f,1,default,1f);
                    }
            for(int i = 0;i<NPC.maxBuffs;i++) {
                if(npc.buffType[i] == ModContent.BuffType<CrystalStackI>() || npc.buffType[i] == ModContent.BuffType<CrystalStackII>() || npc.buffType[i] == ModContent.BuffType<CrystalStackIII>() || npc.buffType[i] == ModContent.BuffType<CrystalStackIIII>()){
                    npc.DelBuff(i);
                    i--;
                }
            }
         }
    }
}
