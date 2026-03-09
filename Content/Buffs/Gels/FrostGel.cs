using ArcaneOdyssey.Content.Buffs.Base;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Buffs.Gels
{
	public class FrostGel : GelBuff
	{
		public override int DebuffID => ModContent.BuffType<FreezingEffect>();

		public override void Effects(Rectangle hitbox)
		{
			var dust = Dust.NewDustDirect(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.SnowflakeIce);
			dust.velocity *= 0.1f;
			dust.noGravity = true;
		}
	}
}
