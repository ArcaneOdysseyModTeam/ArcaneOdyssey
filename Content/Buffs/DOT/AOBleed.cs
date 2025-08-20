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
    public class AOBleed : Base.AODebuff
    {
        private int frameNum = 0;
        System.Random rnd = new System.Random();
        public override void Update(NPC npc, ref int buffIndex) {
            frameNum++;
            if(frameNum>20){
                frameNum = 0;
                CombatText.NewText(npc.Hitbox,CombatText.DamagedHostile,3);
                for(int dustCountInt = 0;dustCountInt<10;dustCountInt++){
                    Dust.NewDust(npc.position+ new Vector2(npc.width/2f,npc.height/2f),1,1,DustID.Blood,(0.5f-rnd.NextSingle())*2f,(0.5f-rnd.NextSingle())*2f,1,default,1f);
                    }
                npc.life-=3;
                if(npc.life<1){
                    //makes npcs die normally
                    npc.life = 1;
                    npc.SimpleStrikeNPC(3,0,false,0f,null,false,0f,false);
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
                    Dust.NewDust(player.position+ new Vector2(player.width/2f,player.height/2f),1,1,DustID.Blood,(0.5f-rnd.NextSingle())*2f,(0.5f-rnd.NextSingle())*2f,1,default,1f);
                    }
        }
    }
}
}
