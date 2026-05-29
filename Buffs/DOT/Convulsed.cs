using ArcaneOdyssey.Buffs.Base;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Buffs.DOT
{
	public class Convulsed : MagicMark
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.pvpBuff[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (!Main.dedServ)
			{
				var dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Wraith);
				dust.velocity *= 0.1f;
				dust.noGravity = true;
			}


			
		}

	}
}