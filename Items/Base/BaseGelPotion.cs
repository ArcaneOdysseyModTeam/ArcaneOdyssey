using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Base
{
	public abstract class BaseGelPotion : BaseItem
	{
		public abstract int GelID { get; }

		public abstract Color LiquidColour { get; }

		public sealed override ItemRarities Rarity => ItemRarities.Rare;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.consumable = true;
			Item.width = 20;
			Item.height = 18;
			Item.useTurn = true;
			Item.maxStack = Item.CommonMaxStack;
			Item.useAnimation = Item.useTime = 17;
			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.UseSound = SoundID.Item3;
			Item.buffType = GelID;
			Item.buffTime = 60 * 60 * 20;
		}

		public sealed override int Value => 7;

		public sealed override string Texture => Mod.Name + "/Assets/GelBottle";
		public Texture2D LiquidSprite => Mod.Assets.Request<Texture2D>("Assets/GelLiquid").Value;

		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			spriteBatch.Draw(LiquidSprite, position, frame, Item.GetAlpha(LiquidColour), 0f, origin, scale, SpriteEffects.None, 0f);
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Main.GetItemDrawFrame(Type, out _, out var itemFrame);
			Vector2 drawOrigin = itemFrame.Size() / 2f;
			Vector2 drawPosition = Item.Bottom - Main.screenPosition - new Vector2(0, drawOrigin.Y);
			spriteBatch.Draw(LiquidSprite, drawPosition, itemFrame, Item.GetAlpha(LiquidColour), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Item.ResearchUnlockCount = 20;
		}
	}
}
