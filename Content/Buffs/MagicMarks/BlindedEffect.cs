using ArcaneOdyssey.Content.Buffs.Base;
using Terraria;
using Terraria.ID;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Buffs.MagicMarks
{
	public class BlindedEffect : AODebuff
	{
		private int stack = 1;
		public override void Update(NPC npc, ref int buffIndex)
		{
			if (!Main.dedServ)
			{
				var dust = Dust.NewDustDirect(npc.position, npc.Hitbox.Width, npc.Hitbox.Height, DustID.YellowStarDust, 0f, 0f, 1, default, 1f);
				dust.velocity *= 0.1f;
				dust.noGravity = true;
			}
			if (npc.HasBuff(Type))
			{
				stack = GetAOBuffStack(npc, buffIndex); // stacks disappear over time
				switch (stack)
				{
					case 1:
						return;
					case 2:
						return;
					case 3:
						return;
					case 4:
						return;
					default:
						npc.AddBuff(BuffID.Confused, 60);
						break;
				}
			}
			if (npc.HasBuff(BuffID.Confused)) {
				npc.DelBuff(buffIndex);
				buffIndex--;
			}
		}

		public override bool ReApply(NPC npc, int time, int buffIndex)
		{
			if (npc.HasBuff(Type))
			{
				npc.buffTime[buffIndex] += time;
				return true;
			}
			else return false;
		}
	}
}
