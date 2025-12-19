using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Materials
{
	[LegacyName("DarkSeaMusicBox", "TitleMusicBox")] // removed items are added here
	public class Acrimony : AOBaseItem
	{
		public int AOValue = 10000;
		public override AORarities AORarity => AORarities.Arcane;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.value = AOUtils.GalleonToCopper(AOValue);
			Item.width = Item.height = 64;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Lighting.AddLight(Item.Center, 3, 3, 3);
			Texture2D texture = TextureAssets.Item[Item.type].Value;
			Main.EntitySpriteDraw(texture, Item.Center - Main.screenPosition, null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None);
			return false;
		}
	}
}
