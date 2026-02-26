using ArcaneOdyssey.Content.Buffs.Base;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Buffs.DOT
{
	public class HeavyBleed : AODebuff
	{
		public override string Texture => $"Terraria/Images/Buff_{BuffID.Bleeding}";
		private int totalTicks = 0;

		public override void Update(NPC npc, ref int buffIndex)
		{
			totalTicks++;
			if (Main.GameUpdateCount % 2 == 0)
			{
				Dust.NewDust(npc.Center, 0, 0, DustID.Blood, Alpha: 1);
			}
			npc.ArcaneOdyssey().bleeding = true;
			if (npc.buffTime[buffIndex] == 2 || (totalTicks / 5) >= 250)
			{
				npc.HitNPC(totalTicks / 5, Main.rand.NextBool().ToDirectionInt());
				for (int dustCountInt = 0; dustCountInt < 30; dustCountInt++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.Blood, Alpha: 1);
				}
				totalTicks = 0;
				npc.DelBuff(buffIndex);
				SoundEngine.PlaySound(SoundID.NPCDeath21, npc.Center);
				buffIndex--;
			}
		}
	}
}
