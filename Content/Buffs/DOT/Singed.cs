using ArcaneOdyssey.Content.Buffs.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Buffs.DOT
{
	public class Singed : AODebuff
	{
		public override string Texture => Mod.Name + "/Assets/Debuff";

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.ArcaneOdyssey().singed = true;
		}
	}
}
