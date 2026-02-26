using ArcaneOdyssey.Content.Buffs.Base;
using System.Collections.Generic;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Buffs.DOT
{
	public class Melting : AODebuff
	{
		public override string Texture => $"Terraria/Images/Buff_{BuffID.Burning}"; 

		public override List<int> Counterparts => [BuffID.OnFire3, BuffID.Burning];
	}
}
