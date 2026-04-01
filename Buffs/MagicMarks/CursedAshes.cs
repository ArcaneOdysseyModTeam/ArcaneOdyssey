using ArcaneOdyssey.Buffs.Base;
using ArcaneOdyssey.Buffs.Stuns;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Buffs.MagicMarks
{
	public class CursedAshes : MagicMark
	{
		public override List<int> Counterparts => [ModContent.BuffType<Petrified>()];

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.ArcaneOdyssey().ashcursed = true;
		}
	}
}
