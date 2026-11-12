using ArcaneOdyssey.Buffs.Base;
using ArcaneOdyssey.Imbues;
using ArcaneOdyssey.Imbues.Base;
using System.Linq;

namespace ArcaneOdyssey.Buffs.MagicMarks
{
	public class Soaked : VanillaClone
	{
		public override int VanillaID => BuffID.Wet;

		public bool vfxSet = false;
		public int dust = DustID.Water;

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
			if (!Main.dedServ)
			{
				if (!vfxSet)
				{
					GetVFXInfo(npc);
					vfxSet = true;
				}
				var spawnedDust = Dust.NewDustDirect(npc.position, npc.width, npc.height, dust);
				spawnedDust.velocity *= 0.4f;
			}
		}

		public override bool ReApply(NPC npc, int time, int buffIndex)
		{
			vfxSet = false;
			return base.ReApply(npc, time, buffIndex);
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (!Main.dedServ)
			{
				var dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.Water);
				dust.velocity *= 0.4f;
			}
		}
	}
}
