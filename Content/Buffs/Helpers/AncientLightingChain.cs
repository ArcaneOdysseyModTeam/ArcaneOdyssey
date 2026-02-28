using ArcaneOdyssey.Content.Buffs.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Lost;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Buffs.Helpers
{
	public class AncientLightingChain : AODebuff
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
						npcs.Center.Distance(npc.Center);
						npcs.AddBuff(Type, 60);
						if (!Main.dedServ)
						{
							ChainVFX(npc.Center, npcs.Center);
						}
					}
				}
				npc.HitNPC(50, Main.rand.NextBool().ToDirectionInt(), ModContent.GetInstance<AncientLightningMagic>(), damageType: DamageClass.Magic, damageVariation: true);
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
	}
}
