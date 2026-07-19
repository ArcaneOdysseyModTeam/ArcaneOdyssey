using ArcaneOdyssey.Buffs.Base;
using System.Collections.Generic;

namespace ArcaneOdyssey.Buffs.DOT
{
	public class Bleeding : VanillaClone
	{
		public override int VanillaID => BuffID.Bleeding;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ExternalModSupport.RegisterDoT(Type);
			Main.pvpBuff[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (Main.GameUpdateCount % 2 == 0)
			{
				Dust.NewDust(npc.Center, 0, 0, DustID.Blood);
			}
			npc.ArcaneOdyssey().bleeding = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (Main.GameUpdateCount % 2 == 0)
			{
				Dust.NewDust(player.Center, 0, 0, DustID.Blood);
			}
			player.ArcaneOdyssey().debuffs.Add(6);
		}

		public override List<int> Counterparts => [.. base.Counterparts, ModContent.BuffType<HeavyBleed>()];
	}
}
