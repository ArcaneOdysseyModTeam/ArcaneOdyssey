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
    public abstract class Stun : AODebuff
    {
        /// <summary>
        /// literally just for custom magics
        /// </summary>
        public virtual bool AffectsBosses => false;
        public override void Update(NPC npc, ref int buffIndex)
        {
            if (!npc.boss || AffectsBosses)
                npc.velocity /= 2;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed = 0f;
            player.canFloatInWater = false;
        }
    }
}
