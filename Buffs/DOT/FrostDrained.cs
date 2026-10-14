using ArcaneOdyssey.Buffs.Base;
using ArcaneOdyssey.Buffs.MagicMarks;
using System.Collections.Generic;

namespace ArcaneOdyssey.Buffs.DOT
{
	public class FrostDrained : MagicMark
	{
		public override void Update(NPC npc, ref int buffIndex)
		{
			if (!Main.dedServ)
			{
				var dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Wraith);
				dust.velocity *= 0.1f;
				dust.noGravity = true;
			}	
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ExternalModSupport.RegisterDoT(Type);
		}


		public override List<int> Counterparts => [ModContent.BuffType<DrainedEffect>(), ModContent.BuffType<FreezingEffect>()];
	}
}
