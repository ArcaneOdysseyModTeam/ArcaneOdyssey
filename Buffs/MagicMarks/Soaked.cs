using ArcaneOdyssey.Buffs.Base;

namespace ArcaneOdyssey.Buffs.MagicMarks
{
	public class Soaked : VanillaClone
	{
		public override int VanillaID => BuffID.Wet;

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (!Main.dedServ)
			{
				var dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Water);
				dust.velocity *= 0.4f;
			}
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (!Main.dedServ)
			{
				var dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.Water);
				dust.velocity *= 0.4f;
			}
		}
	}
}
