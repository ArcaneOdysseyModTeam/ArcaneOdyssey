using ArcaneOdyssey.Buffs.Base;
using ArcaneOdyssey.Buffs.MagicMarks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Buffs.Gels
{
	public class ArcticGel : GelBuff
	{
		public override int DebuffID => ModContent.BuffType<SnowyEffect>();

		public override void Effects(Rectangle hitbox)
		{
			var dust = Dust.NewDustDirect(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.SnowBlock);
			dust.velocity *= 0.1f;
			dust.noGravity = true;
		}
	}
}
