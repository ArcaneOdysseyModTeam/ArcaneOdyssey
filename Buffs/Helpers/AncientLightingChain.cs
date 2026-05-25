using ArcaneOdyssey.Buffs.Base;
using ArcaneOdyssey.Imbues.Magic.Lost;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Buffs.Helpers
{
	public class AncientLightingChain : MagicMark
	{
		public const float ChainDistance = 200f;
		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.ArcaneOdyssey().ZapCD < 0)
			{
				npc.ArcaneOdyssey().ZapCD = 5 * 60;

				foreach (var npcs in Main.ActiveNPCs)
				{
					if (npcs.Center.Distance(npc.Center) <= ChainDistance && (!npc.friendly))
					{
						npcs.AddBuff(Type, npc.buffTime[buffIndex]);
						if (!Main.dedServ)
						{
							ChainVFX(npc.Center, npcs.Center);
						}
					}
				}
				if (AOUtils.ServerOrSingleplayer)
					npc.HitNPC(npc.buffTime[buffIndex], Main.rand.NextBool().ToDirectionInt(), ModContent.GetInstance<AncientLightningMagic>(), damageType: DamageClass.Magic, damageVariation: true);
			}
			npc.DelBuff(buffIndex);
			buffIndex--;
		}

		public static void ChainVFX(Vector2 start, Vector2 end)
		{
			Vector2 currentPosition = start;
			for (int n = 0; n < 20; n++)
			{
				currentPosition += new Vector2(MathF.Cos(start.AngleTo(end)), MathF.Sin(start.AngleTo(end))) * (start.Distance(end) / 20f);
				Dust spawnedDust = Dust.NewDustPerfect(currentPosition + new Vector2(0f, GetWaveVal(n)).RotatedBy(start.AngleTo(end)), DustID.TheDestroyer, Vector2.Zero, 255, Color.Red, 1.2f);
				spawnedDust.noGravity = true;
			}
		}

		private static float GetWaveVal(float timestamp)
		{
			return 10f * MathF.Abs(timestamp % 5 % 10f - 2.5f) - 12.5f;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ArcaneOdyssey().ZapCD < 0)
			{
				player.ArcaneOdyssey().ZapCD = 5 * 60;

				foreach (var players in Main.ActivePlayers)
				{
					if (players.Center.Distance(player.Center) <= ChainDistance)
					{
						players.AddBuff(Type, player.buffTime[buffIndex]);
						if (!Main.dedServ)
						{
							ChainVFX(player.Center, players.Center);
						}
					}
				}

				if (Main.myPlayer == player.whoAmI)
				{
					player.Hurt(PlayerDeathReason.ByCustomReason(Mod.CustomLocalization($"{LocalizationCategory}.{Name}.Death", player.name).ToNetworkText()), player.buffTime[buffIndex], Main.rand.NextBool().ToDirectionInt(), dodgeable: false, knockback: 0f, scalingArmorPenetration: 1f);
				}
			}
			player.DelBuff(buffIndex);
			buffIndex--;
		}
	}
}
