using ArcaneOdyssey.Content.Buffs.Base;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Buffs.DOT
{
	public class AOPoisoned : AODebuff
	{
		public override string Texture => $"Terraria/Images/Buff_{BuffID.Poisoned}";
		public override int[] Counterparts => [BuffID.Poisoned, ModContent.BuffType<ElectrifiedToxins>()];
	}
}
