using ArcaneOdyssey.Buffs.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Buffs.DOT
{
	public class VesuvianBurn : MagicMark
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
				Lighting.AddLight(npc.Center, 1f, 0.19f, 0f);
			}
			npc.ArcaneOdyssey().vesuvianBurn = true;
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ExternalModSupport.RegisterDoT(Type);
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.wet && !player.lavaWet)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
				return;
			}
			if (!Main.dedServ)
			{
				Dust.NewDust(player.position, player.width, player.height, DustID.UltraBrightTorch, 0f, 0f, 0, new Color(0, 0, 255, 0), 1.2f);
				Dust.NewDust(player.position, player.width, player.height, DustID.SolarFlare, 0f, 0f, 0, Color.Blue, 1.2f);
				Lighting.AddLight(player.Center, 1f, 0.19f, 0f);
			}
			player.ArcaneOdyssey().debuffs.Add(60);
		}
	}
}
