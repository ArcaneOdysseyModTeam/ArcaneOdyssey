using ArcaneOdyssey.Content.Buffs.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Buffs.DOT
{
	public class ElectrifiedToxins : AODebuff
	{
		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.ArcaneOdyssey().elecToxins = true;
		}
	}
}
