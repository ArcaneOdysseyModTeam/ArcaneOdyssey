using ArcaneOdyssey.Buffs.Base;
using Terraria.Audio;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Buffs.DOT
{
	public class HeavyBleed : MagicMark
	{
		public override string Texture => $"Terraria/Images/Buff_{BuffID.Bleeding}";
		private int totalTicks = 0;

		public override void Update(NPC npc, ref int buffIndex)
		{
			totalTicks++;
			if (Main.GameUpdateCount % 2 == 0)
			{
				Dust.NewDust(npc.Center, 0, 0, DustID.Blood);
			}
			npc.ArcaneOdyssey().bleeding = true;
			if (npc.buffTime[buffIndex] == 2 || (totalTicks / 5) >= 250)
			{
				npc.HitNPC(totalTicks / 5, Main.rand.NextBool().ToDirectionInt());
				for (int dustCountInt = 0; dustCountInt < 30; dustCountInt++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.Blood);
				}
				totalTicks = 0;
				npc.DelBuff(buffIndex);
				SoundEngine.PlaySound(SoundID.NPCDeath21, npc.Center);
				buffIndex--;
			}
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ExternalModSupport.RegisterDoT(Type);
		}

		public override void Update(Player player, ref int buffIndex)
		{
			totalTicks++;
			player.ArcaneOdyssey().debuffs.Add(6);
			if (Main.GameUpdateCount % 2 == 0)
			{
				Dust.NewDust(player.Center, 0, 0, DustID.Blood);
			}
			if (player.buffTime[buffIndex] == 2 || (totalTicks / 5) >= 250)
			{
				player.Hurt(PlayerDeathReason.ByCustomReason(Mod.CustomLocalization($"{LocalizationCategory}.{Name}.Death", player.name).ToNetworkText()), totalTicks/5, Main.rand.NextBool().ToDirectionInt(), quiet: true, dodgeable: false, scalingArmorPenetration: 1f, knockback: 0f);
				for (int dustCountInt = 0; dustCountInt < 30; dustCountInt++)
				{
					Dust.NewDust(player.Center, 0, 0, DustID.Blood);
				}
				totalTicks = 0;
				player.DelBuff(buffIndex);
				SoundEngine.PlaySound(SoundID.NPCDeath21, player.Center);
				buffIndex--;
			}
		}
	}
}
