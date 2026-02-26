using ArcaneOdyssey.Content.Buffs.Base;
using System.Collections.Generic;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Buffs.DOT
{
	public class Corroding : AODebuff
	{
		public override string Texture => $"Terraria/Images/Buff_{BuffID.Venom}"; 
		public override List<int> Counterparts => [BuffID.Venom];
	}
}
