using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
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
                var dust = Dust.NewDustDirect(npc.position, npc.Hitbox.Width, npc.Hitbox.Height, DustID.CursedTorch, 0f, -1f, 1, default, 3f);
                dust.noGravity = true;
                dust.velocity *= 0.8f;
            }
            npc.lifeRegen -= 25 + (30 * GetBurnStacks(npc));
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
