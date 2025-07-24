using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Buffs
{
	public class RisenTide : ModBuff
	{
		public override void SetStaticDefaults() 
		{
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}
		
        public override void Update(Player player, ref int buffIndex)
        {
            player.lavaImmune = true;
        }
	}
}
