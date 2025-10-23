using ArcaneOdyssey.Content.Mounts;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Buffs.Mounts
{
	public class RowboatBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.mount.SetMount(ModContent.MountType<Rowboat>(), player);
			player.buffTime[buffIndex] = 10;
		}
	}
}
