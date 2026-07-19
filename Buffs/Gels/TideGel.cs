using ArcaneOdyssey.Buffs.Base;
using ArcaneOdyssey.Buffs.MagicMarks;

namespace ArcaneOdyssey.Buffs.Gels
{
	public class TideGel : GelBuff
	{
		public override int DebuffID => ModContent.BuffType<Soaked>();

		public override void Effects(Rectangle hitbox)
		{
			var dust = Dust.NewDustDirect(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.Water);
			dust.velocity *= 0.4f;
		}
	}
}
