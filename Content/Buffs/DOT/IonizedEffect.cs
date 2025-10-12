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
using ArcaneOdyssey.Content.Buffs.MagicMarks;

namespace ArcaneOdyssey.Content.Buffs.DOT
{
    public class IonizedEffect : AODebuff
    {
        public override void Update(NPC npc, ref int buffIndex)
        {
            if (!Main.dedServ)
            {
                var dust = Dust.NewDustDirect(npc.position + new Vector2((float)npc.width / 2f, (float)npc.height / 2f), 1, 1, DustID.CursedTorch, 0f, 0f, 1, default, 3f);
                dust.velocity *= 0.8f;
            }
            npc.lifeRegen -= 15 + (10 * GetBurnStacks(npc));
        }
        private int BurnStack(int buff, NPC npc)
        {
            if (npc.HasBuff(buff))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        private int GetBurnStacks(NPC npc)
        {
            int burnCount = 0;
            burnCount = 0;
            burnCount += BurnStack(BuffID.OnFire, npc);
            burnCount += BurnStack(BuffID.ShadowFlame, npc);
            burnCount += BurnStack(BuffID.OnFire3, npc);
            burnCount += BurnStack(BuffID.CursedInferno, npc);
            burnCount += BurnStack(BuffID.Daybreak, npc);
            burnCount += BurnStack(BuffID.Frostburn, npc);
            burnCount += BurnStack(BuffID.Oiled, npc);
            burnCount += BurnStack(BuffID.Slimed, npc);
            burnCount += BurnStack(ModContent.BuffType<CharredEffect>(), npc);
            burnCount += BurnStack(ModContent.BuffType<SearedEffect>(), npc);
            return burnCount;
        }
    }
}
