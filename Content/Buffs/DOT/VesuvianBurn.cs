using ArcaneOdyssey.Content.Buffs.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Buffs.DOT
{
	public class VesuvianBurn : AODebuff
	{
		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.wet && !npc.lavaWet)
			{
				npc.DelBuff(buffIndex);
				buffIndex--;
				return;
			}
			if (!Main.dedServ)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.UltraBrightTorch, 0f, 0f, 0, new Color(0, 0, 255, 0), 1.2f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.SolarFlare, 0f, 0f, 0, Color.Blue, 1.2f);
				Lighting.AddLight(npc.position, 1f, 0.19f, 0f);
			}
			npc.ArcaneOdyssey().vesuvianBurn = true;
		}
	}
}
