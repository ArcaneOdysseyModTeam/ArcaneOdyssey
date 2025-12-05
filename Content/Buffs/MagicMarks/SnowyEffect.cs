using ArcaneOdyssey.Content.Buffs.Base;
using Terraria.ID;
using Terraria;

namespace ArcaneOdyssey.Content.Buffs.MagicMarks
{
	public class SnowyEffect : AODebuff
	{
		public override void Update(NPC npc, ref int buffIndex)
		{
		   if (!Main.dedServ)
			{
				var dust = Dust.NewDustDirect(npc.position, npc.Hitbox.Width, npc.Hitbox.Height, DustID.SnowBlock, 0f, 0f, 1, default, 1f);
				dust.velocity *= 0.1f;
				dust.noGravity = true;
			}   
		}
	}
}
