using ArcaneOdyssey.Content.Buffs;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Scrolls.Usable.Lost
{
	public class EnchantmentSpell : LostScroll
	{
		public override bool CanHaveMagic => true;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.mana = 200;
			Item.useTime = Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.HoldUp;
		}

		public override void UseAnimation(Player player)
		{
			if (Main.netMode != NetmodeID.SinglePlayer && Main.myPlayer == player.whoAmI)
			{
				ActivateAbility(player);
				ChatHelper.SendChatMessageFromClient(new ChatMessage($"[c/{Color.AliceBlue.Hex3()}:{Mod.CustomLocalization("RandomWords.Enchantment", player.name)}]"));
				foreach (var players in Main.ActivePlayers)
				{
					if (players.whoAmI != player.whoAmI)
						players.AddBuff(ModContent.BuffType<Enchanted>(), 60 * 60 * 5, false); // 5 mins
				}
			}
			else
			{
				Item.SetDefaults(ModContent.ItemType<EmptyScroll>());
			}
			AOMagic.CreateMagicCircle(Item, player, Imbue);
		}
	}
}
