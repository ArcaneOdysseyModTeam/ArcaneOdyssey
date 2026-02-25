using ArcaneOdyssey.Content.Buffs.Base;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Buffs.DOT
{
	public class AOBurning : VanillaClone
	{
		public override int VanillaID => BuffID.OnFire;
		public override int[] Counterparts => [BuffID.OnFire];
	}
}
