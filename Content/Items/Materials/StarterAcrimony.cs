using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Materials
{
	public class StarterAcrimony : AOBaseItem
	{
		public override AORarities AORarity => AORarities.Arcane;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 64;
		}

		public override LocalizedText DisplayName => Mod.CustomLocalization($"{LocalizationCategory}.{nameof(Acrimony)}.DisplayName");
		public override LocalizedText Tooltip => Mod.CustomLocalization($"{LocalizationCategory}.{nameof(Acrimony)}.Tooltip");
        public override string Texture => (GetType().Namespace + "." + nameof(Acrimony)).Replace('.', '/');

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Lighting.AddLight(Item.Center, 3, 3, 3);
			Texture2D texture = TextureAssets.Item[Item.type].Value;
			Main.EntitySpriteDraw(texture, Item.Center - Main.screenPosition, null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None);
			return false;
		}
	}
}
