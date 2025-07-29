using ArcaneOdyssey.Content.Buffs;
using ArcaneOdyssey.Content.Items.Base;
using Steamworks;
using System.Linq.Expressions;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static ArcaneOdyssey.AOUtils;

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
                npc.life-=3;
                CombatText.NewText(npc.Hitbox,CombatText.DamagedHostile,3);
                for(int dustCountInt = 0;dustCountInt<10;dustCountInt++){
                    Dust.NewDust(npc.position+ new Vector2((float)npc.width/2f,(float)npc.height/2f),1,1,DustID.Blood,(0.5f-rnd.NextSingle())*2f,(0.5f-rnd.NextSingle())*2f,1,default,1f);
                    }
            }
        }
    }
}
