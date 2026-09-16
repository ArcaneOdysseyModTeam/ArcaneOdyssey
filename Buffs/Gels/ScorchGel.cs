using ArcaneOdyssey.Buffs.Base;
using ArcaneOdyssey.Buffs.DOT;

namespace ArcaneOdyssey.Buffs.Gels
{
	public class ScorchGel : GelBuff
	{
		public override int DebuffID => ModContent.BuffType<Scorched>();

		public override void Effects(Rectangle hitbox)
		{
			var dust = Dust.NewDustDirect(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.Shadowflame);
			dust.velocity *= 0.4f;
		}
	}
}
