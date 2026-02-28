using ArcaneOdyssey.Content.Buffs.Base;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Buffs.MagicMarks
{
	public class Flammable : VanillaClone
	{
		public override int VanillaID => BuffID.Oiled;

		public override List<int> Counterparts => [..base.Counterparts, BuffID.Slimed, BuffID.GelBalloonBuff];

		public override string Texture => AOUtils.GetTexture<Flammable>();

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.oiled = true;
			//if (!Main.dedServ)
			//{
			//	var dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Water_Cavern);
			//	dust.velocity *= 0.4f;
			//}
		}
	}
}
