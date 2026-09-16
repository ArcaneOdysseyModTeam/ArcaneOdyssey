using ArcaneOdyssey.Buffs.Base;

namespace ArcaneOdyssey.Buffs.DOT
{
	public class SearedEffect : MagicMark
	{
		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.wet && !npc.lavaWet)
			{
				npc.DelBuff(buffIndex);
				buffIndex--;
				return;
			}
			Dust newDust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.CrimsonTorch, (0.5f - Main.rand.NextFloat()) * 2f, (0.5f - Main.rand.NextFloat()) * 2f, 1, default, 3f);
			newDust.noGravity = true;
			npc.ArcaneOdyssey().seared = true;
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ExternalModSupport.RegisterDoT(Type);
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.wet && !player.lavaWet)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
				return;
			}
			Dust newDust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.CrimsonTorch, (0.5f - Main.rand.NextFloat()) * 2f, (0.5f - Main.rand.NextFloat()) * 2f, 1, default, 3f);
			newDust.noGravity = true;
			player.ArcaneOdyssey().debuffs.Add(10);
		}
	}
}
