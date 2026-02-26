using ArcaneOdyssey.Content.Buffs.Base;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Buffs.DOT
{
	public class Scorched : AODebuff
	{
		public override string Texture => $"Terraria/Images/Buff_{BuffID.Electrified}";

		public override List<int> Counterparts => [BuffID.ShadowFlame, ModContent.BuffType<AOShadowflame>()];

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.shadowFlame = true;
		}
	}
}
