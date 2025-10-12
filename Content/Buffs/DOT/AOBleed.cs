using ArcaneOdyssey.Content.Items.Base;
using Steamworks;
using System.Linq.Expressions;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static ArcaneOdyssey.AOUtils;
using Terraria.Localization;
using ArcaneOdyssey.Content.Buffs.Base;

namespace ArcaneOdyssey.Content.Buffs.DOT
{
    public class AOBleed : AODebuff
    {
        private int frameNum = 0;
        public override void Update(NPC npc, ref int buffIndex) 
        {
            frameNum++;
            if (frameNum>20)
            {
                frameNum = 0;
                for(int dustCountInt = 0;dustCountInt<10;dustCountInt++)
                {
                    Dust.NewDust(npc.position + new Vector2(npc.width/2f,npc.height/2f),1,1,DustID.Blood,(0.5f-Main.rand.NextFloat())*2f,(0.5f-Main.rand.NextFloat())*2f,1,default,1f);
                }
            }
            npc.ArcaneOdyssey().Bleeding = true;
        }
    }
}
