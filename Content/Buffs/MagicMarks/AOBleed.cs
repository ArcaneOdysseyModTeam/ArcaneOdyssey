using ArcaneOdyssey.Content.Buffs;
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
                if(npc.life<1){
                    //makes npcs die normally
                    npc.life = 1;
                    npc.SimpleStrikeNPC(3,0,false,0f,null,false,0f,false);
                }
                CombatText.NewText(npc.Hitbox,CombatText.DamagedHostile,3);
                for(int dustCountInt = 0;dustCountInt<10;dustCountInt++){
                    Dust.NewDust(npc.position+ new Vector2((float)npc.width/2f,(float)npc.height/2f),1,1,DustID.Blood,(0.5f-rnd.NextSingle())*2f,(0.5f-rnd.NextSingle())*2f,1,default,1f);
                    }
            }
        }
        public override void Update(Player player, ref int buffIndex)
        {
            Player.HurtInfo info;
			frameNum++;
            if(frameNum>20){
                frameNum = 0;
                player.statLife-=3;
                if(player.statLife<0)
                {
                    player.Hurt(PlayerDeathReason.ByCustomReason(Mod.CustomLocalization("Buffs.AOBleed.Death", [player.name]).ToNetworkText()), 1, 0, out info, false, false, -1, false,0f, 0f,0f);
                }
                CombatText.NewText(player.Hitbox,CombatText.DamagedFriendly,3);
                for(int dustCountInt = 0;dustCountInt<10;dustCountInt++){
                    Dust.NewDust(player.position+ new Vector2((float)player.width/2f,(float)player.height/2f),1,1,DustID.Blood,(0.5f-rnd.NextSingle())*2f,(0.5f-rnd.NextSingle())*2f,1,default,1f);
                    }
        }
    }
}
}
