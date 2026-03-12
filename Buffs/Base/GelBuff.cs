using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Buffs.Base
{
	public abstract class GelBuff : AOBaseBuff
	{
		public const int meleeEnchantID = 99;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.pvpBuff[Type] = true;
			Main.meleeBuff[Type] = true;
			Main.persistentBuff[Type] = true;
			BuffID.Sets.IsAFlaskBuff[Type] = true;
		}

		public abstract int DebuffID { get; }

		public override LocalizedText DisplayName => Mod.CustomLocalization("RandomWords.WeaponGel", AOUtils.GetBuffName(DebuffID));
		public override LocalizedText Description => Mod.CustomLocalization("RandomWords.GelTooltip", AOUtils.GetBuffName(DebuffID));

		public override void Update(Player player, ref int buffIndex)
		{
			player.ArcaneOdyssey().Gel = this;
			player.meleeEnchant = meleeEnchantID;
		}

		public abstract void Effects(Rectangle hitbox);
	}
}
