using ArcaneOdyssey.Items.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Items.Consumable
{
	public class AtlanteanEssence : BaseItem
	{
		public override Rarities Rarity => Rarities.Mystic;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 20;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.HiddenAnimation;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Lighting.AddLight(Item.Center, 1, 0, 1);
			return true;
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ItemNoGravity[Type] = true;
		}

		public override void UseAnimation(Player player) => RightClick(player);

		public override bool CanRightClick() => true;

		public override void RightClick(Player player)
		{
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

				Item.TurnToAir();
				break;
			}
		}

		public override bool ConsumeItem(Player player) => false;
	}
}
