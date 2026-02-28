using ArcaneOdyssey.Content.Buffs.Base;
using ArcaneOdyssey.Content.Buffs.DOT;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Buffs.Gels
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
