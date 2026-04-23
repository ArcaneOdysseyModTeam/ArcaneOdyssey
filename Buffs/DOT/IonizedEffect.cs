using ArcaneOdyssey.Buffs.Base;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Buffs.DOT
{
	public class IonizedEffect : MagicMark
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
				var dust = Dust.NewDustDirect(npc.position, npc.Hitbox.Width, npc.Hitbox.Height, DustID.CursedTorch, 0f, -1f, 1, default, 3f);
				dust.noGravity = true;
				dust.velocity *= 0.8f;
			}
			npc.ArcaneOdyssey().ionized = true;
		}

		public override List<int> Counterparts => [BuffID.CursedInferno];

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ExternalModSupport.RegisterDoT(Type);
		}
	}
}
