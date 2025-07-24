using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Buffs
{
	public class RisingTide : ModBuff
	{
		private int currentFrameThingy;
		public override void SetStaticDefaults() 
		{
			currentFrameThingy = 0;
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

        public override void Update(Player player, ref int buffIndex)
        {
			player.statDefense += 20;
			if(currentFrameThingy > 4){
				player.direction = System.Math.Sign(player.direction*-1);
				currentFrameThingy = 0;
			}
			currentFrameThingy++;
        }
	}
}
