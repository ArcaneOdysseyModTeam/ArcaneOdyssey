using ArcaneOdyssey.Content.Buffs.Base;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Buffs.MagicMarks
{
	public class FreezingEffect : AODebuff
	{
		public override List<int> Counterparts => [BuffID.Chilled];
		public override string Texture => $"Terraria/Images/Buff_{BuffID.Chilled}";
		public override void Update(NPC npc, ref int buffIndex)
		{
			if (!Main.dedServ)
			{
				var dust = Dust.NewDustDirect(npc.position, npc.Hitbox.Width, npc.Hitbox.Height, DustID.SnowflakeIce, 0f, 0f, 1, default, 1f);
				dust.velocity *= 0.1f;
				dust.noGravity = true;
			}
		}
	}
}
