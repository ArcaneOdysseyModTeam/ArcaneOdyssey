using ArcaneOdyssey.Content.Buffs.Base;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Buffs.DOT
{
	public class IonizedEffect : AODebuff
	{
		public override void Update(NPC npc, ref int buffIndex)
		{
			if (!Main.dedServ)
			{
				var dust = Dust.NewDustDirect(npc.position, npc.Hitbox.Width, npc.Hitbox.Height, DustID.CursedTorch, 0f, -1f, 1, default, 3f);
				dust.noGravity = true;
				dust.velocity *= 0.8f;
			}
			npc.ArcaneOdyssey().ionized = true;
		}
	}
}
