using ArcaneOdyssey.Buffs.Base;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Buffs.DOT
{
	public class AOBurning : VanillaClone
	{
		public override int VanillaID => BuffID.OnFire;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ExternalModSupport.RegisterDoT(Type);
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.wet && !npc.lavaWet)
			{
				npc.DelBuff(buffIndex);
				buffIndex--;
				return;
			}
			npc.ArcaneOdyssey().burning = true;
			if (!Main.dedServ)
			{
				var dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Torch);
				dust.velocity *= 0.4f;
			}
		}
	}
}
