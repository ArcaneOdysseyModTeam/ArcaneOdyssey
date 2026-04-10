using ArcaneOdyssey.Items.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Items.Materials
{
	public class AncientHecateOrb : BaseItem
	{
		public override int Value => 20000;
		public override Rarities Rarity => Rarities.Mythical;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 32;
		}
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ItemNoGravity[Type] = true;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Lighting.AddLight(Item.Center, 2, 0, 2);
			scale = .8f;
			return true;
		}
	}
}
