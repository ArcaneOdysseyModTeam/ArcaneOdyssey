using ArcaneOdyssey.Buffs.Base;
using ArcaneOdyssey.Buffs.MagicMarks;

namespace ArcaneOdyssey.Buffs.Gels
{
	public class DesertGel : GelBuff
	{
		public override int DebuffID => ModContent.BuffType<SandyEffect>();

		public override void Effects(Rectangle hitbox)
		{
			var dust = Dust.NewDustDirect(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.Sand);
			dust.velocity *= 0.1f;
			dust.noGravity = true;
		}
	}
}
