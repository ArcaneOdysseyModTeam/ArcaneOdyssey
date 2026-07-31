using ArcaneOdyssey.NPCs.Bosses;

namespace ArcaneOdyssey.Buffs
{
	public class ThunderingPresence : ModBuff
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.debuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = true; 
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if(!AOUtils.NPCAlive<LordElius>())
			{
				player.ClearBuff(ModContent.BuffType<ThunderingPresence>());
			}
		}
	}
}
