using ArcaneOdyssey.Buffs;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.EmptyScrolls;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Scrolls.Usable.Rare
{
	public class EnchantmentSpell : RareScroll
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
			Imbuable.CreateMagicCircle(Item, player, Projectiles.MagicCircleMode.Rotating, true);
		}
	}
}
