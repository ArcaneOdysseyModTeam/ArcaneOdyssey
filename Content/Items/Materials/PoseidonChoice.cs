using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Materials
{
	[LegacyName("HecateOrb")]
	public class PoseidonChoice : AOBaseItem
	{
		public int AOValue = 0;
		public override AORarities AORarity => AORarities.Arcane;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.value = GalleonToCopper(AOValue);
			Item.width = Item.height = 64;
		}

		public override LocalizedText DisplayName => Mod.CustomLocalization($"{LocalizationCategory}.{nameof(PoseidonSpirit)}.DisplayName");
		public override LocalizedText Tooltip => Mod.CustomLocalization($"{LocalizationCategory}.{nameof(PoseidonSpirit)}.Tooltip");

		public override void SetStaticDefaults()
		{
			ItemID.Sets.CanGetPrefixes[Type] = false;
			ItemID.Sets.ItemNoGravity[Item.type] = true;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = TextureAssets.Item[Item.type].Value;
			Main.EntitySpriteDraw(texture, Item.Center - Main.screenPosition, null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None);
			return false;
		}
	}
}
