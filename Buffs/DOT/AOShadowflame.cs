using ArcaneOdyssey.Buffs.Base;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Buffs.DOT
{
	public class AOShadowflame : VanillaClone
	{
		public override int VanillaID => BuffID.ShadowFlame;

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.ArcaneOdyssey().shadowflame = true;
			if (!Main.dedServ)
			{
				var dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Shadowflame);
				dust.velocity *= 0.4f;
			}
		}
	}
}
