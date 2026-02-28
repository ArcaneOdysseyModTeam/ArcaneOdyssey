using ArcaneOdyssey.Content.Buffs.Base;
using ArcaneOdyssey.Content.Buffs.DOT;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Buffs.Gels
{
	public class BleedGel : GelBuff
	{
		public override int DebuffID => ModContent.BuffType<AOBleed>();

		public override void Effects(Rectangle hitbox)
		{
			if (Main.GameUpdateCount % 2 == 0)
			{
				Dust.NewDust(hitbox.Center(), 0, 0, DustID.Blood);
			}
		}
	}
}
