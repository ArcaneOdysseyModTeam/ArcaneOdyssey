using ArcaneOdyssey.Buffs.Base;
using ArcaneOdyssey.Imbues;
using ArcaneOdyssey.Imbues.Base;
using System.Collections.Generic;
using System.Linq;

namespace ArcaneOdyssey.Buffs.MagicMarks
{
	public class SandyEffect : MagicMark
	{
		public override List<int> Counterparts => [BuffID.WindPushed];

		public void GetVFXInfo(NPC npc)
		{
			foreach (var imbueID in ImbueID.Sets.ImbuesThatCreateBuff[Type])
			{
				var _ = Color.White;
				if (AOUtils.Singleplayer)
				{
					if (Main.LocalPlayer.HasTypeInInventory<MagicType>(out var imbue, e => e.ID == imbueID && e.magicVariant is not null))
					{
						imbue.magicVariant.BuffVisuals(ref dust, ref _, ref _);
					}
				}
				else
				{
					var players = Main.player.Where(e => e is not null && e.active).OrderBy(e => e.Distance(npc.Center));
					foreach (var player in players)
					{
						if (player.HasTypeInInventory<MagicType>(out var imbue, e => e.ID == imbueID && e.magicVariant is not null))
						{
							imbue.magicVariant.BuffVisuals(ref dust, ref _, ref _);
							break;
						}
					}
				}
			}
		}

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
					GetVFXInfo(npc);
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
