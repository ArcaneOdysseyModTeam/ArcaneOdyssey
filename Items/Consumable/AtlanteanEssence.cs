using ArcaneOdyssey.GlobalTypes;
using ArcaneOdyssey.Items.Base;

namespace ArcaneOdyssey.Items.Consumable
{
	public class AtlanteanEssence : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Mystic;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 20;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.HiddenAnimation;
			Item.maxStack = Item.CommonMaxStack;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Lighting.AddLight(Item.Center, 1, 0, 1);
			return true;
		}

		public static ref Asset<Texture2D> AtlanteanIndicator => ref AOItem.AtlanteanIndicator;

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			if (AOUtils.RequestIfExists(Mod.Name + "/Assets/AtlanteanIndicator", ref AtlanteanIndicator))
			{
				spriteBatch.Draw(AtlanteanIndicator.Value, position, null, Item.GetAlpha(Color.White), 0, AtlanteanIndicator.Size() / 2f, Main.inventoryScale * 1.1f, SpriteEffects.None, 1f);
			}
			return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ItemNoGravity[Type] = true;
			ItemID.Sets.ItemIconPulse[Type] = true;
		}

		public override void UseAnimation(Player player) => RightClick(player);

		public override bool CanRightClick() => true;

		private bool consumerism = false;

		public override void RightClick(Player player)
		{
			consumerism = false;
			for (int i = 0; i < player.inventory.Length; i++)
			{
				var item = player.inventory[i];
				if (item is null || item.ArcaneOdyssey() is null)
				{
					continue;
				}

				if (!item.ArcaneOdyssey().AddAtlanteanEssense())
				{
					continue;
				}

				consumerism = true;
				break;
			}
		}

		public override bool ConsumeItem(Player player) => consumerism;
	}
}
