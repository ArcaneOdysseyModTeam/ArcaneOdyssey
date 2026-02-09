using ArcaneOdyssey.Content.Buffs.Base;
using ArcaneOdyssey.Content.Buffs.DOT;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Buffs.Gels
{
	public class BleedGel : GelBuff
	{
		public override int DebuffID => ModContent.BuffType<AOBleed>();
	}
}
