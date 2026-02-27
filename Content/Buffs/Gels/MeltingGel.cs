using ArcaneOdyssey.Content.Buffs.Base;
using ArcaneOdyssey.Content.Buffs.DOT;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Buffs.Gels
{
	public class MeltingGel : GelBuff
	{
		public override int DebuffID => ModContent.BuffType<Melting>();
		public override string Texture => AOUtils.GelTexture;
	}
}
