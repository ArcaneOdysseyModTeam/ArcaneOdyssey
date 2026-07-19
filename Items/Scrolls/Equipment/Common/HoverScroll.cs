using ArcaneOdyssey.Items.Base;
using Terraria.GameContent;

namespace ArcaneOdyssey.Items.Scrolls.Equipment.Common
{
	public class HoverScroll : CommonScroll
	{
		public override bool CanHaveMagic => true;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (HasCorrectImbue)
			{
				player.carpet = true;
				player.GetModPlayer<HoverPlayer>().hasHoverEquipped = true;
				if (player.carpetTime > 0 && player.controlJump)
				{
					player.moveSpeed += Imbue.ScrollSpeed.MultiToPercent();
					Imbue.LingeringEffects(player.Hitbox);
				}
				else
					player.carpetTime = (player.carpetTime * Imbue.ScrollDamage).Round();
			}
		}

		public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
		{
			return incomingItem.type != ItemID.FlyingCarpet;
		}
	}

	public class HoverPlayer : ModPlayer
	{
		public bool hasHoverEquipped = false;

		public override void PostUpdateMiscEffects()
		{
			if ((!Main.dedServ) && Main.myPlayer == Player.whoAmI)
			{
				Asset<Texture2D> carpetNoneLol = Mod.Assets.Request<Texture2D>("Assets/BlankCarpet");
				Asset<Texture2D> carpetOriginal = IHATECARPETS.carpet;
				TextureAssets.FlyingCarpet = hasHoverEquipped ? carpetNoneLol : carpetOriginal;
			}
		}

		public override void ResetEffects()
		{
			hasHoverEquipped = false;
		}
	}

	public class IHATECARPETS : ModSystem
	{
		public static Asset<Texture2D> carpet;
		public override void Load()
		{
			carpet = TextureAssets.FlyingCarpet;
		}
	}
}
