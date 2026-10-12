using ArcaneOdyssey.Buffs.Base;
using ArcaneOdyssey.Imbues.Magic.Normal;
using System.Linq;

namespace ArcaneOdyssey.Buffs.MagicMarks
{
	public class Soaked : VanillaClone
	{
		public override int VanillaID => BuffID.Wet;

		public bool vfxSet = false;
		public int dust = DustID.Water;

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (!Main.dedServ)
			{
				if (!vfxSet)
				{
					if (Main.netMode == NetmodeID.SinglePlayer)
					{
						if (Main.LocalPlayer.HasTypeInInventory<WaterMagic>(out var imbue))
						{
							WaterMagic.GetVFX(imbue, out dust);
						}
					}
					else
					{
						var players = Main.player.Where(e => e.active).OrderBy(e => e.Distance(npc.Center));
						foreach (var player in players)
						{
							if (player is not null && player.HasTypeInInventory<WaterMagic>(out var imbue))
							{
								WaterMagic.GetVFX(imbue, out dust);
								break;
							}
						}
					}
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
