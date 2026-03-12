using ArcaneOdyssey.Buffs.Base;
using ArcaneOdyssey.Mounts;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Buffs.Mounts
{
	public class RowboatBuff : AOBaseBuff
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
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
