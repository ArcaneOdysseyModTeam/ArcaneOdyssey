using ArcaneOdyssey.Content.Buffs.Base;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Buffs.DOT
{
	public class AOBurning : AODebuff
	{
		public override string Texture => $"Terraria/Images/Buff_{BuffID.OnFire}";
		public override int[] Counterparts => [BuffID.OnFire];
	}
}
