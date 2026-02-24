using ArcaneOdyssey.Content.Buffs;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Scrolls.Weapons.Lost
{
	public class EnchantmentSpell : LostScroll
	{
		public override bool CanHaveMagic => true;

		public override string Texture => AOUtils.GetTexture<AnnihilationScroll>();

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.mana = 200;
			Item.useTime = Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.HoldUp;
		}

		public override void UseAnimation(Player player)
		{
			player.AddBuff(ModContent.BuffType<Enchanted>(), 60 * 60 * 5); // 5 mins
			if (Main.dedServ)
			{
				ChatHelper.BroadcastChatMessage(Mod.CustomLocalization("RandomWords.Enchantment", player.name).ToNetworkText(), Color.AliceBlue);
			}
			else if (Main.netMode == NetmodeID.SinglePlayer)
			{
				Main.NewText(Mod.CustomLocalization("RandomWords.Enchantment", player.name).Value, Color.AliceBlue);
			}
		}
	}
}
