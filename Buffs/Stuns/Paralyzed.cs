using ArcaneOdyssey.Buffs.Base;
using ArcaneOdyssey.Buffs.DOT;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Buffs.Stuns
{
	public class Paralyzed : Stun 
	{
		public override List<int> Counterparts => [BuffID.Electrified, ModContent.BuffType<ElectrifiedToxins>()];
		public override string Texture => $"Terraria/Images/Buff_{BuffID.Electrified}";
	}
}
