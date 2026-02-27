using ArcaneOdyssey.Content.Buffs.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Buffs.DOT
{
	public class ElectrifiedToxins : AODebuff
	{
		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.ArcaneOdyssey().elecToxins = true;
			if (!Main.dedServ)
			{
				var dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.SteampunkSteam, newColor: Color.Purple);
				dust.velocity *= 0.4f;
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.WitherLightning, newColor: Color.Purple);
			}
		}
	}
}
