using ArcaneOdyssey.Buffs.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Buffs.DOT
{
	public class ElectrifiedToxins : MagicMark
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

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ExternalModSupport.RegisterDoT(Type);
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.ArcaneOdyssey().elecToxins = true;
			if (!Main.dedServ)
			{
				var dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.SteampunkSteam, newColor: Color.Purple);
				dust.velocity *= 0.4f;
				Dust.NewDust(player.position, player.width, player.height, DustID.WitherLightning, newColor: Color.Purple);
			}
		}
	}
}
