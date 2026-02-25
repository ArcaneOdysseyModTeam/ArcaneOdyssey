using ArcaneOdyssey.Content.Buffs.Base;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Buffs.DOT
{
	public class AOBleed : AODebuff
	{
		public override string Texture => $"Terraria/Images/Buff_{BuffID.Bleeding}";

		public override int[] Counterparts => [BuffID.Bleeding];

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (Main.GameUpdateCount % 2 == 0) 
			{
				Dust.NewDust(npc.Center, 0, 0, DustID.Blood, Alpha: 1);
			}
			npc.ArcaneOdyssey().bleeding = true;
		}
	}
}
