using ArcaneOdyssey.Content.Buffs.Base;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Buffs.Stuns
{
	public class Paralyzed : Stun 
	{
		public override int[] Counterparts => [BuffID.Electrified];
	}
}
