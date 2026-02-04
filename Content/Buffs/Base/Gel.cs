using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Buffs.Base
{
	public abstract class GelBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			BuffID.Sets.IsAFlaskBuff[Type] = true;
		}

		public abstract int DebuffID { get; }

		public override void Update(Player player, ref int buffIndex)
		{
			player.ArcaneOdyssey().gel = DebuffID;
		}
	}
}
