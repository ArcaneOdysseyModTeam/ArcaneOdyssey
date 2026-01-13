using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Materials
{
	[LegacyName("DarkSeaMusicBox", "TitleMusicBox")] // removed items are added here
	public class Acrimony : AOBaseItem
	{
		public int AOValue = 10000;
		public override AORarities AORarity => AORarities.Legendary;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ItemNoGravity[Type] = true;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.value = AOUtils.GalleonToCopper(AOValue);
			Item.width = Item.height = 32;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Lighting.AddLight(Item.Center, 3, 3, 3);
			return true;
		}
	}
}
