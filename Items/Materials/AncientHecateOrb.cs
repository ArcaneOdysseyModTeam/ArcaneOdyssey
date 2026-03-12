using ArcaneOdyssey.Items.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Items.Materials
{
	public class AncientHecateOrb : AOBaseItem
	{
		public int AOValue = 20000;
		public override AORarities AORarity => AORarities.Mythical;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.value = AOUtils.GalleonToCopper(AOValue);
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
