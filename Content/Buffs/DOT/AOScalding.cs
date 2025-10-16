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
    public class AOScalding : AODebuff
    {
        public override void Update(NPC npc, ref int buffIndex)
        {
            if (!Main.dedServ)
            {
                var dust = Dust.NewDustDirect(npc.position + new Vector2(npc.width / 2f, npc.height / 2f), 1, 1, DustID.SteampunkSteam, 0f, 0f, 1, default, 1f);
                dust.velocity *= 0.4f;
            }
            npc.ArcaneOdyssey().Scalding = true;
        }
    }
}
