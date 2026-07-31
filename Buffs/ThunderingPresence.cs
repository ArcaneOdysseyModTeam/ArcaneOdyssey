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
	}
}
