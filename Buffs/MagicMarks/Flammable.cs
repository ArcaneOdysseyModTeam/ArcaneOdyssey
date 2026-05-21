using ArcaneOdyssey.Buffs.Base;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Buffs.MagicMarks
{
	public class Flammable : VanillaClone
	{
		public override int VanillaID => BuffID.Oiled;

		public override List<int> Counterparts => [.. base.Counterparts, BuffID.Slimed, BuffID.GelBalloonBuff];

		public override string Texture => AOUtils.GetTexture<Flammable>();

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.wet && !npc.lavaWet)
			{
				npc.DelBuff(buffIndex);
				buffIndex--;
				return;
			}
			npc.oiled = true;
			//if (!Main.dedServ)
			//{
			//	var dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Water_Cavern);
			//	dust.velocity *= 0.4f;
			//}
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.ArcaneOdyssey().oiled = true;
			if (!Main.dedServ)
			{
				var dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.Water_Cavern);
				dust.velocity *= 0.4f;
			}
		}
	}
}
