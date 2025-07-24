using ArcaneOdyssey.Content.Buffs.MagicMarks;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace ArcaneOdyssey.Content.Buffs.Stuns
{
    /// <summary>
    /// nobody will tell its a custom debuff thats the point lol
    /// </summary>
    public class AOFrozen : MagicMark
    {
        public override void Update(NPC npc, ref int buffIndex)
        {
            // npc.SimpleFlyMovement(new Vector2(0, npc.maxFallSpeed), npc.maxFallSpeed);
            npc.velocity /= 4;
        }
    }
}
