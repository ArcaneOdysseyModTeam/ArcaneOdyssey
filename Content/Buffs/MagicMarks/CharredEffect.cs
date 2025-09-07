using ArcaneOdyssey.Content.Buffs.Base;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Buffs.MagicMarks
{
    public class CharredEffect : AODebuff
    {
        public override void Update(NPC npc, ref int buffIndex)
        {
            Dust.NewDust(npc.position + new Vector2((float)npc.width / 2f, (float)npc.height / 2f), 1, 1, DustID.Smoke, 0f, 0f, 1, default, 1f);   
        }
    }
}
