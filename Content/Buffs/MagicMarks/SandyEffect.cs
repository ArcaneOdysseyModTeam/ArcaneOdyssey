using ArcaneOdyssey.Content.Buffs.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;

namespace ArcaneOdyssey.Content.Buffs.MagicMarks
{
    public class SandyEffect : AODebuff
    {
        public override void Update(NPC npc, ref int buffIndex)
        {
            Dust.NewDust(npc.position + new Vector2((float)npc.width / 2f, (float)npc.height / 2f), 1, 1, DustID.Sand, (0.5f - Main.rand.NextFloat()) * 0.1f, 0f, 1, default, 1f);
        }
    }
}
