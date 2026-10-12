using ArcaneOdyssey.Buffs.Base;
using ArcaneOdyssey.Imbues.Magic.Normal;
using System.Collections.Generic;
using System.Linq;

namespace ArcaneOdyssey.Buffs.MagicMarks
{
	public class SandyEffect : MagicMark
	{
		public override List<int> Counterparts => [BuffID.WindPushed];

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.wet && !npc.lavaWet)
			{
				if (AOUtils.ServerOrSingleplayer)
				{
					npc.DelBuff(buffIndex);
					buffIndex--;
				}
				return;
			}
			if (!Main.dedServ)
			{
				if (!vfxSet)
				{
					if (Main.netMode == NetmodeID.SinglePlayer)
					{
						if (Main.LocalPlayer.HasTypeInInventory<SandMagic>(out var imbue))
						{
							SandMagic.GetVFX(imbue, out dust);
						}
					}
					else
					{
						var players = Main.player.Where(e => e.active).OrderBy(e => e.Distance(npc.Center));
						foreach (var player in players)
						{
							if (player is not null && player.HasTypeInInventory<SandMagic>(out var imbue))
							{
								SandMagic.GetVFX(imbue, out dust);
								break;
							}
						}
					}
					vfxSet = true;
				}
				var spawnedDust = Dust.NewDustDirect(npc.position, npc.width, npc.height, dust);
				spawnedDust.velocity *= 0.1f;
				spawnedDust.noGravity = true;
			}
		}
		public override bool ReApply(NPC npc, int time, int buffIndex)
		{
			vfxSet = false;
			return base.ReApply(npc, time, buffIndex);
		}

		private bool vfxSet = false;
		public int dust = DustID.Sand;

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.wet && !player.lavaWet)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
				return;
			}
			if (!Main.dedServ)
			{
				var dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.Sand);
				dust.velocity *= 0.1f;
				dust.noGravity = true;
			}
		}
	}
}
