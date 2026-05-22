using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using Terraria;
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
			if (Main.netMode != NetmodeID.SinglePlayer)
			{
				if (Main.myPlayer == player.whoAmI)
				{
					Imbuable.CreateMagicCircle(Item, player, Projectiles.MagicCircleMode.Rotating, true);
					ActivateAbility(player);
					var packet = Mod.GetPacket();
					packet.Write(ArcaneOdysseyMod.PacketID.Enchantment);
					packet.Send();
				}
			}
			else
			{
				Item.SetDefaults(ItemID.Sets.ShimmerTransformToItem[Type]);
			}
		}
	}
}
