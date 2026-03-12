using ArcaneOdyssey.Buffs.Base;
using ArcaneOdyssey.Buffs.DOT;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Buffs.Gels
{
	public class MeltingGel : GelBuff
	{
		public override int DebuffID => ModContent.BuffType<Melting>();

		public override void Effects(Rectangle hitbox)
		{
			var dust = Dust.NewDustDirect(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.Lava);
			dust.velocity *= 0.4f;
		}
	}
}
