using ArcaneOdyssey.Content.Buffs.Base;
using ArcaneOdyssey.Content.Buffs.DOT;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Buffs.Stuns
{
	public class Paralyzed : Stun 
	{
		public override int[] Counterparts => [BuffID.Electrified, ModContent.BuffType<ElectrifiedToxins>()];
		public override string Texture => $"Terraria/Images/Buff_{BuffID.Electrified}";
	}
}
