using ArcaneOdyssey.Buffs.Base;

namespace ArcaneOdyssey.Buffs.DOT
{
	public class Shadowflame : VanillaClone
	{
		public override int VanillaID => BuffID.ShadowFlame;

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.ArcaneOdyssey().shadowflame = true;
			if (!Main.dedServ)
			{
				var dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Shadowflame);
				dust.velocity *= 0.4f;
			}
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ExternalModSupport.RegisterDoT(Type);
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.ArcaneOdyssey().debuffs.Add(18);
			if (!Main.dedServ)
			{
				var dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.Shadowflame);
				dust.velocity *= 0.4f;
			}
		}
	}
}
