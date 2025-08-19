using ArcaneOdyssey.Content.Buffs.Base;
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
    public abstract class Stun : MagicMark
    {
        public override void Update(NPC npc, ref int buffIndex)
        {
            if (!npc.boss)
                npc.velocity /= 2;
        }
    }
}
