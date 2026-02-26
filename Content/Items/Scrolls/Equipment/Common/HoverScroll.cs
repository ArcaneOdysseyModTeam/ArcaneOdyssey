using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Scrolls.Equipment.Common
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
					player.moveSpeed += Imbue.AOScrollSpeed.MultiToPercent();
					Imbue.LingeringEffects(player.Hitbox);
				}
				else
					player.carpetTime = (player.carpetTime * Imbue.AOScrollDamage).Round();
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
				Asset<Texture2D> carpetNoneLol = ModContent.Request<Texture2D>($"{Mod.Name}/Assets/BlankCarpet");
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
