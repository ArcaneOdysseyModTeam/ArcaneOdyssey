using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items
{
	[LegacyName("HecateOrb", "PoseidonChoice")]
	public class EagleLegacy : AOBaseItem
	{
		public override AORarities AORarity => AORarities.Mythical;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 64;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.useAnimation = 20;
			Item.noUseGraphic = true;
			Item.useTime = 20;
		}

		public override void SetStaticDefaults()
		{
			ItemID.Sets.ItemNoGravity[Type] = true;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = TextureAssets.Item[Type].Value;
			Main.EntitySpriteDraw(texture, Item.Center - Main.screenPosition, null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None);
			return false;
		}

		#region UI system
		public override bool CanUseItem(Player player)
		{
			try
			{
				//Main.NewText($"Can use item {!ModContent.GetInstance<ImbueChangeUISystem>().CanShowImbueAcquire()}");
				return !ModContent.GetInstance<ImbueAnythingUISystem>().CanShowImbueAcquire();
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
			try { ModContent.GetInstance<ImbueAnythingUISystem>().ShowAcquireUI(); }
			// Spoky (2026 Jan 25): By the way, I like putting exceptions in purple
			catch (Exception ex) { Main.NewText($"Error in {nameof(UseItem)}: \n{ex}", new Color(255, 0, 255)); }
			return true;
		}
		#endregion
	}
}
