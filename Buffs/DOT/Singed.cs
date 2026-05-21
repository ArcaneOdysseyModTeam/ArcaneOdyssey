using ArcaneOdyssey.Buffs.Base;
using Terraria;
using Terraria.ID;


namespace ArcaneOdyssey.Buffs.DOT
{
	public class Singed : MagicMark
	{
		private int stack = 1;

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.wet && !npc.lavaWet)
			{
				npc.DelBuff(buffIndex);
				buffIndex--;
				return;
			}

			stack = AOUtils.GetAOBuffStack(npc, buffIndex); // stacks disappear over time
			npc.ArcaneOdyssey().singedstacks = stack;

			if (!Main.dedServ)
			{
				Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.CrimsonTorch, (0.5f - Main.rand.NextFloat()) * 2f, (0.5f - Main.rand.NextFloat()) * 2f, 1, default, 3f);
			}
		}

		public override bool ReApply(NPC npc, int time, int buffIndex)
		{
			if (npc.HasBuff(Type))
			{
				npc.buffTime[buffIndex] = Utils.Clamp(npc.buffTime[buffIndex] + time, 0, 20 * 5 * 60);
				return true;
			}
			else return false;
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ExternalModSupport.RegisterDoT(Type);
		}

		public override void Update(Player player, ref int buffIndex)
		{
			stack = AOUtils.GetAOBuffStack(player, buffIndex); // stacks disappear over time
			player.ArcaneOdyssey().singe = stack;

			if (!Main.dedServ)
			{
				Dust.NewDustDirect(player.position, player.width, player.height, DustID.CrimsonTorch, (0.5f - Main.rand.NextFloat()) * 2f, (0.5f - Main.rand.NextFloat()) * 2f, 1, default, 3f);
			}
		}
	}
}
