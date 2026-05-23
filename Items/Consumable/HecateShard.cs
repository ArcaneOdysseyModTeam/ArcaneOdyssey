using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Consumable
{
	public class HecateShard : BaseItem
	{
		public override int Value => 20000;
		public override ItemRarities Rarity => ItemRarities.Legendary;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 32;

			Item.useStyle = ItemUseStyleID.HiddenAnimation;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.noUseGraphic = true;
		}
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ItemIconPulse[Type] = true;
			ItemID.Sets.ItemNoGravity[Type] = true;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Lighting.AddLight(Item.Center, new Vector3(2, 0, 2) * scale);
			return true;
		}


		#region UI system
		public override bool CanUseItem(Player player)
		{
			try
			{
				//Main.NewText($"Can use item {!ModContent.GetInstance<ImbueAnythingUISystem>().CanShowImbueSequelAcquire()}");
				ModUISystem instance = ModContent.GetInstance<ModUISystem>();

				// Spoky (2026 May 23): I have no idea why this gives error, but I'll just put this failsafe just in case it catches an error
				if (instance is null) 
					return false;

				if (instance.CanShowMutations())
					return false;

				foreach (var i in player.inventory)
					if (i.ModItem is MagicType magic && magic.ImbuableTier == ImbuableTiers.Normal)
						return true;
				return false;
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
					ModContent.GetInstance<ModUISystem>().ShowMutationUI();
					Main.playerInventory = false;
				}
			}
			// Spoky (2026 Jan 25): By the way, I like putting exceptions in purple
			catch (Exception ex) { Main.NewText($"Error in {nameof(UseItem)}: \n{ex}", new Color(255, 0, 255)); }
			return true;
		}
		#endregion
	}
}
