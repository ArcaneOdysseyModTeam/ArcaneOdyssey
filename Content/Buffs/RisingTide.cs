using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Buffs
{
	public class RisingTide : ModBuff
	{
		public override void SetStaticDefaults() 
		{
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

        public override void Update(Player player, ref int buffIndex)
        {
			player.statDefense += 20;
			player.direction = (player.direction !> 0).ToDirectionInt();
        }
	}
}
