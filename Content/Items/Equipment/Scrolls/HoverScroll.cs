using ArcaneOdyssey.Content.Items.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArcaneOdyssey.Content.Items.Materials;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace ArcaneOdyssey.Content.Items.Equipment.Scrolls
{
	public class HoverScroll : MagicScroll
	{
		public override int AOValue => 1000;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
		}

		public override void UpdateEquip(Player player)
		{
			if (player.TryGetImbue(out var imbue) && imbue is AOMagic)
			{
				player.carpet = true;
				player.GetModPlayer<HoverPlayer>().hasHoverEquipped = true;
				if (player.carpetTime > 0 && player.controlJump)
				{
					player.moveSpeed += imbue.AOScrollSpeed.MultiToPercent();
					imbue.LingeringEffects(player);
				}
				else
					player.carpetTime = (player.carpetTime * imbue.AOScrollDamage).Round();
			}
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<EmptyScroll>().AddIngredient(ItemID.FlyingCarpet).Register();
			CreateRecipe().AddIngredient<EmptyScroll>().AddIngredient(ItemID.SandstorminaBottle).Register();
			CreateRecipe().AddIngredient<EmptyScroll>().AddRecipeGroup(RecipeGroupID.SandstormBalloons).Register();
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
				Asset<Texture2D> carpetNoneLol = ModContent.Request<Texture2D>($"{ArcaneOdysseyMod.InternalName}/Assets/BlankCarpet");
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
