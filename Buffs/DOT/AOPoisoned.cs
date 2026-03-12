using ArcaneOdyssey.Buffs.Base;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Buffs.DOT
{
	public class AOPoisoned : VanillaClone
	{
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
	}
}
