using ArcaneOdyssey.Content.Buffs.Base;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Buffs.DOT
{
	public class AOBurning : VanillaClone
	{
		public override int VanillaID => BuffID.OnFire;

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.ArcaneOdyssey().burning = true;
			if (!Main.dedServ)
			{
				var dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Torch);
				dust.velocity *= 0.4f;
			}
		}
	}
}
