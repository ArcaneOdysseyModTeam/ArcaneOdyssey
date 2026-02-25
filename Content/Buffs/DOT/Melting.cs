using ArcaneOdyssey.Content.Buffs.Base;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace ArcaneOdyssey.Content.Buffs.DOT
{
	public class Melting : AODebuff
	{
		public override string Texture => $"Terraria/Images/Buff_{BuffID.Burning}"; 

		public override int[] Counterparts => [BuffID.OnFire3, BuffID.Burning];
	}
}
