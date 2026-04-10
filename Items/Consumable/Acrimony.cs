using ArcaneOdyssey.Items.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Consumable
{
	[LegacyName("TitleMusicBox", "StarterAcrimony")] // rare removed items are added here
	public class Acrimony : BaseItem
	{
		public override int Value => 10000;
		public override Rarities Rarity => Rarities.Legendary;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ItemNoGravity[Type] = true;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 32;
			//Item.useStyle = ItemUseStyleID.HoldUp;
			//Item.useAnimation = 20;
			//Item.useTime = 20;
		}

		// Spoky (2026 Fev 08): Removed this function from acrimony to the magics (fighting style and/or eagle patrimony); If you read this then it meas I forgot to delete this
		//public override bool CanUseItem(Player player)
		//{
		//	try
		//	{
		//		//Main.NewText($"Can use item {!ModContent.GetInstance<MagicChoiceUISystem>().CanShowUI()}");
		//		return !ModContent.GetInstance<MagicChoiceUISystem>().CanShowUI();
		//	}
		//	catch (Exception ex) 
		//	{
		//		Main.NewText($"Error in {nameof(CanUseItem)}: \n{ex}", new Color(255, 0, 255));
		//		return false; 
		//	}
		//}

		//public override bool? UseItem(Player player)
		//{
		//	// Spoky (2026 Jan 25): Expected for errors to have an error message but it appears we don't have said luxury, therefore gotta get errors, manually
		//	try { ModContent.GetInstance<MagicChoiceUISystem>().ShowUI(); }
		//	// Spoky (2026 Jan 25): By the way, I like putting exceptions in purple
		//	catch (Exception ex) { Main.NewText($"Error in {nameof(UseItem)}: \n{ex}", new Color(255, 0, 255)); }
		//	return true;
		//}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Lighting.AddLight(Item.Center, 3, 3, 3);
			return true;
		}
	}
}
