using ArcaneOdyssey.Content.Buffs.Base;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Buffs.DOT
{
	public class AOBleed : VanillaClone
	{
		public override int VanillaID => BuffID.Bleeding;

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (Main.GameUpdateCount % 2 == 0) 
			{
				Dust.NewDust(npc.Center, 0, 0, DustID.Blood);
			}
			npc.ArcaneOdyssey().bleeding = true;
		}

		public override List<int> Counterparts => [..base.Counterparts, ModContent.BuffType<HeavyBleed>()];
	}
}
