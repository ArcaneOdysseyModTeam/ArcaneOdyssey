using ArcaneOdyssey.Buffs.Base;
using ArcaneOdyssey.Imbues.Magic.Normal;
using System.Collections.Generic;
using System.Linq;

namespace ArcaneOdyssey.Buffs.DOT
{
	public class Poisoned : VanillaClone
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ExternalModSupport.RegisterDoT(Type);
		}
		public override int VanillaID => BuffID.Poisoned;
		public override List<int> Counterparts => [.. base.Counterparts, ModContent.BuffType<ElectrifiedToxins>()];
		private bool vfxSet = false;
		public Color dustColour = Color.Purple;

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.ArcaneOdyssey().poisoned = true;
			if (!Main.dedServ)
			{
				if (!vfxSet)
				{
					if (Main.netMode == NetmodeID.SinglePlayer)
					{
						if (Main.LocalPlayer.HasTypeInInventory<PoisonMagic>(out var imbue))
						{
							dustColour = imbue.Colour;
						}
					}
					else
					{
						var players = Main.player.Where(e => e.active).OrderBy(e => e.Distance(npc.Center));
						foreach (var player in players)
						{
							if (player is not null && player.HasTypeInInventory<PoisonMagic>(out var imbue))
							{
								dustColour = imbue.Colour;
								break;
							}
						}
					}
					vfxSet = true;
				}
				var dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.SteampunkSteam, newColor: dustColour);
				dust.velocity *= 0.4f;
			}
		}

		public override bool ReApply(NPC npc, int time, int buffIndex)
		{
			vfxSet = false;
			return base.ReApply(npc, time, buffIndex);
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.poisoned = true;
		}
	}
}
