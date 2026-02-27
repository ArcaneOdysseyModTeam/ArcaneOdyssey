using ArcaneOdyssey.Content.Buffs.Base;
using ArcaneOdyssey.Content.Buffs.DOT;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Buffs.Gels
{
	public class ScorchGel : GelBuff
	{
		public override int DebuffID => ModContent.BuffType<Scorched>();
		public override string Texture => AOUtils.GelTexture;
	}
}
