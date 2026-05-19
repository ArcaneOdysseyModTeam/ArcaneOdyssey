using ArcaneOdyssey.Buffs.Base;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Buffs.MagicMarks
{
	public class BlindedEffect : MagicMark
	{
		public override List<int> Counterparts => [BuffID.Confused];
		public override void Update(NPC npc, ref int buffIndex)
		{
			if (!Main.dedServ)
			{
				var dust = Dust.NewDustDirect(npc.position, npc.Hitbox.Width, npc.Hitbox.Height, DustID.YellowStarDust);
				dust.velocity *= 0.1f;
				dust.noGravity = true;
			}

			var stack = AOUtils.GetAOBuffStack(npc, buffIndex); // stacks disappear over time
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

			if (npc.HasBuff(BuffID.Confused))
			{
				npc.DelBuff(buffIndex);
				buffIndex--;
			}
		}

		public override bool ReApply(NPC npc, int time, int buffIndex)
		{
			npc.buffTime[buffIndex] += time;
			return true;
		}

		public override bool ReApply(Player player, int time, int buffIndex)
		{
			player.buffTime[buffIndex] += time;
			return true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (!Main.dedServ)
			{
				var dust = Dust.NewDustDirect(player.position, player.Hitbox.Width, player.Hitbox.Height, DustID.YellowStarDust);
				dust.velocity *= 0.1f;
				dust.noGravity = true;
			}
			if (player.HasBuff(Type))
			{
				var stack = AOUtils.GetAOBuffStack(player, buffIndex); // stacks disappear over time
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
						player.AddBuff(BuffID.Confused, 60);
						break;
				}
			}
			if (player.HasBuff(BuffID.Confused))
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}
}
