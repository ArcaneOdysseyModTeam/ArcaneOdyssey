using ArcaneOdyssey.Content.Buffs.Base;
using ArcaneOdyssey.Content.Buffs.DOT;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Buffs.Gels
{
	public class CorrodingGel : GelBuff
	{
		public override int DebuffID => ModContent.BuffType<Corroding>();
		public override string Texture => AOUtils.GelTexture;
	}
}
