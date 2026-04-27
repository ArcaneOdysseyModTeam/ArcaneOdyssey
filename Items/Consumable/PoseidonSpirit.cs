using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Consumable;

public class PoseidonSpirit : BaseItem
{
	public override int Value => 10000;
	public override ItemRarities Rarity => ItemRarities.Mythical;

	public override void SetDefaults()
	{
		base.SetDefaults();
		Item.width = Item.height = 64;
		Item.useStyle = ItemUseStyleID.HiddenAnimation;
		Item.useAnimation = 20;
		Item.useTime = 20;
		Item.noUseGraphic = true;
	}

	public override void SetStaticDefaults()
	{
		ItemID.Sets.ItemNoGravity[Type] = true;
	}

	public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
	{
		Texture2D texture = TextureAssets.Item[Type].Value;
		spriteBatch.Draw(texture, Item.Center - Main.screenPosition, null, Item.GetAlpha(Color.White), rotation, Vector2.Zero, scale / 2f, SpriteEffects.None, 0f);
		return false;
	}

	public override void UpdateInventory(Player player)
	{
		player.ArcaneOdyssey().AddSoul(AOPlayers.GodSoulID.Poseidon);
	}

	#region UI system
	public override bool CanUseItem(Player player)
	{
		try
		{
			//Main.NewText($"Can use item {!ModContent.GetInstance<ImbueAnythingUISystem>().CanShowImbueSequelAcquire()}");
			return !ModContent.GetInstance<ModUISystem>().CanShowImbueSequelAcquire();
		}
		catch (Exception ex)
		{
			Main.NewText($"Error in {nameof(CanUseItem)}: \n{ex}", new Color(255, 0, 255));
			return false;
		}
	}
	public override bool? UseItem(Player player)
	{
		// Spoky (2026 Jan 25): Expected for errors to have an error message but it appears we don't have said luxury, therefore gotta get errors, manually
		try
		{
			if (player.whoAmI == Main.myPlayer)
			{
				ModContent.GetInstance<ModUISystem>().ShowAcquireSequelUI();
				Main.playerInventory = false;
			}
		}
		// Spoky (2026 Jan 25): By the way, I like putting exceptions in purple
		catch (Exception ex) { Main.NewText($"Error in {nameof(UseItem)}: \n{ex}", new Color(255, 0, 255)); }
		return true;
	}
	#endregion
}
