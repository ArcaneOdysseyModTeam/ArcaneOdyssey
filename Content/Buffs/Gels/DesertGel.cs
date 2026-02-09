using ArcaneOdyssey.Content.Buffs.Base;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Buffs.Gels
{
	public class DesertGel : GelBuff
	{
		public override int DebuffID => ModContent.BuffType<SandyEffect>();
	}
}
