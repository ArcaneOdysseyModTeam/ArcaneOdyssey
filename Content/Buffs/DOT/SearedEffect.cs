using Terraria;
using Terraria.ID;
using ArcaneOdyssey.Content.Buffs.Base;

namespace ArcaneOdyssey.Content.Buffs.DOT
{
	public class SearedEffect : AODebuff
	{
		public override void Update(NPC npc, ref int buffIndex)
		{
			Dust newDust = Dust.NewDustDirect(npc.position, npc.Hitbox.Width, npc.Hitbox.Height, DustID.CrimsonTorch, (0.5f - Main.rand.NextFloat()) * 2f, (0.5f - Main.rand.NextFloat()) * 2f, 1, default, 3f);
			newDust.noGravity = true;
			npc.ArcaneOdyssey().seared = true;
		}
	}
}
