using ArcaneOdyssey.Content.Buffs.Base;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Buffs.DOT
{
	public class AOShadowflame : VanillaClone
	{
		public override int VanillaID => BuffID.ShadowFlame;

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.shadowFlame = true;
		}
	}
}
