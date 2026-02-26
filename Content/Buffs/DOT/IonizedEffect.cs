using ArcaneOdyssey.Content.Buffs.Base;
using ArcaneOdyssey.Content.Buffs.Helpers;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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

		private static int GetBurnStacks(NPC npc)
		{
			int BurnStack(int buff)
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
			int EZBurnStack<T>() where T : AOBaseBuff
			{
				int[] buffs = [ModContent.BuffType<T>(), ..ModContent.GetInstance<T>().Counterparts];
				foreach (var buff in buffs)
				{
					if (npc.HasBuff(buff))
					{
						return 1;
					}
				}
				return 0;
			}
			int burnCount = 0;
			burnCount += BurnStack(BuffID.OnFire);
			burnCount += BurnStack(BuffID.ShadowFlame);
			burnCount += BurnStack(BuffID.OnFire3);
			burnCount += BurnStack(BuffID.CursedInferno);
			burnCount += BurnStack(BuffID.Daybreak);
			burnCount += BurnStack(BuffID.Frostburn);
			burnCount += BurnStack(BuffID.Oiled);
			burnCount += BurnStack(BuffID.Slimed);
			burnCount += BurnStack(ModContent.BuffType<CharredEffect>());
			burnCount += BurnStack(ModContent.BuffType<SearedEffect>());
			burnCount += BurnStack(ModContent.BuffType<PhoenixHealing>());
			burnCount += BurnStack(ModContent.BuffType<VesuvianBurn>());
			burnCount += BurnStack(ModContent.BuffType<ProminenceDebuff>());
			burnCount += BurnStack(ModContent.BuffType<Singed>());
			// add more here later
			return burnCount;
		}
	}
}
