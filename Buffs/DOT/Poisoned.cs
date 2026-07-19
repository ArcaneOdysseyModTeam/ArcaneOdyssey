using ArcaneOdyssey.Buffs.Base;
using System.Collections.Generic;

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

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.ArcaneOdyssey().poisoned = true;
			if (!Main.dedServ)
			{
				var dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.SteampunkSteam, newColor: Color.Purple);
				dust.velocity *= 0.4f;
			}
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.poisoned = true;
		}
	}
}
