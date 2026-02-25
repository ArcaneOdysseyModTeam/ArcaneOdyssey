using ArcaneOdyssey.Content.Buffs.Base;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Buffs.DOT
{
	public class MagicShadowflame : AODebuff
	{
		public override string Texture => $"Terraria/Images/Buff_{BuffID.ShadowFlame}";
		public override int[] Counterparts => [BuffID.ShadowFlame];
	}
}
