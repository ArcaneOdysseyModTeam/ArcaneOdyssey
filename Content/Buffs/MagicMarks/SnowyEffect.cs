using ArcaneOdyssey.Content.Buffs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria;

namespace ArcaneOdyssey.Content.Buffs.MagicMarks
{
    public class SnowyEffect : AODebuff
    {
        public override void Update(NPC npc, ref int buffIndex)
        {
           if (Main.netMode != NetmodeID.Server) Dust.NewDust(npc.position + new Vector2((float)npc.width / 2f, (float)npc.height / 2f), 1, 1, DustID.SnowBlock, 0f, 0f, 1, default, 1f);   
        }
    }
}
