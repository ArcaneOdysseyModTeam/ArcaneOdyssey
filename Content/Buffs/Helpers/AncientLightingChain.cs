using ArcaneOdyssey.Content.Buffs.Base;
using ArcaneOdyssey.Content.Items.Weapons;
using ArcaneOdyssey.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Buffs.Helpers
{
	public class AncientLightingChain : AODebuff
	{
		public const float ChainDistance = 200f;
		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.ArcaneOdyssey().ZapCD < 0)
			{
				npc.ArcaneOdyssey().ZapCD = 5f;

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
				npc.SimpleStrikeNPC(50, Main.rand.NextBool().ToDirectionInt(), damageType: DamageClass.Magic, damageVariation: true);
			}

			npc.DelBuff(buffIndex);
			buffIndex--;
		}

		public static void ChainVFX(Vector2 start, Vector2 end)
		{
			
		}
	}
}
