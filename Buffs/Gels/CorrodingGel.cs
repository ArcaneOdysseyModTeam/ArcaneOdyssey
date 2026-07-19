using ArcaneOdyssey.Buffs.Base;
using ArcaneOdyssey.Buffs.DOT;

namespace ArcaneOdyssey.Buffs.Gels
{
	public class CorrodingGel : GelBuff
	{
		public override int DebuffID => ModContent.BuffType<Corroding>();

		public override void Effects(Rectangle hitbox)
		{
			var dust = Dust.NewDustDirect(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.Water_Corruption);
			dust.velocity *= 0.4f;
		}
	}
}
