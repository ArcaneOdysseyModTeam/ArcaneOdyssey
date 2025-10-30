using ArcaneOdyssey.Content.Buffs.Base;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Buffs.MagicMarks
{
    public class PheonixHealing : AODebuff
    {
        public const float HealDistance = 200f;
        public override void Update(NPC npc, ref int buffIndex)
        {
            foreach (var player in Main.ActivePlayers)
            {
                if (npc.Hitbox.Distance(player.Center) <= 800)
                {
                    player.ArcaneOdyssey().pheonixHealing += 1;
                    if (!Main.dedServ)
                        HealEffect(player, npc);
                }
            }
        }

        public static void HealEffect(Player player, NPC npc)
        {
            if (Main.GameUpdateCount % 3 == 0)
            {
                Dust.NewDust(player.position, player.width, player.height, DustID.BlueTorch, Scale: 1.5f);
                Dust.NewDust(player.position, player.width, player.height, DustID.OrangeTorch, Scale: 1.5f);
            }
            foreach (var i in Enumerable.Range(0, player.MountedCenter.Distance(npc.Center).Round()))
            {
                if (i % 16 == 0)
                {
                    var dust = Dust.NewDustPerfect(npc.Center + (i * npc.Center.DirectionTo(player.MountedCenter)), Main.rand.Next(new int[] { DustID.BlueTorch, DustID.OrangeTorch }), (npc.Center.DirectionTo(player.MountedCenter) * 2.4f) + player.velocity.SafeNormalize(Vector2.Zero));
                    dust.noGravity = true;
                }
            }
        }
    }
}
