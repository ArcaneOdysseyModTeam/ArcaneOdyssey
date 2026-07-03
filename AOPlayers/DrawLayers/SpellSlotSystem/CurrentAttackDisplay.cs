using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.EmptyScrolls;
using Fargowiltas.Common.Configs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace ArcaneOdyssey.AOPlayers.DrawLayers.SpellSlotSystem
{
	public class CurrentAttackDisplay : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => PlayerDrawLayers.AfterLastVanillaLayer;

		public static Asset<Texture2D> backgroundSprite;
		public override void Load()
		{
			backgroundSprite = Mod.Assets.Request<Texture2D>("Assets/GelBuffBackground");
		}

		[JITWhenModsEnabled("Fargowiltas")]
		public static float Opacity => FargoClientConfig.Instance.DebuffOpacity;

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			if (drawInfo.shadow == 0f)
			{
				Player player = drawInfo.drawPlayer;
				if (player.DeadOrGhost)
					return;

				if (Main.gameMenu)
					return;

				if (player.whoAmI != Main.myPlayer)
					return;

				if (player.PlayerItem().ModItem is Imbuable imbue)
				{
					var spell = imbue.selectedAttack;
					if (spell is not null)
					{
						Vector2 offset;

						var colour = Color.White;

						if (ExternalModSupport.HasFargos)
						{
							offset = new Vector2(Player.defaultWidth / 2f, -64);
							colour *= Opacity;
						}
						else
						{
							offset = new Vector2(Player.defaultWidth / 2f, -32);
							colour *= .75f;
						}
						var pos = drawInfo.Position + offset;

						Texture2D texture = backgroundSprite.Value;
						DrawData a = new(texture, pos - Main.screenPosition, texture.Frame(), colour, 0f, texture.Size() / 2f, 1f, SpriteEffects.None, 0);

						Asset<Texture2D> tex;
						if (spell.Scroll != 0)
							tex = TextureAssets.Item[spell.Scroll];
						else
							tex = TextureAssets.Item[imbue.Type];

						DrawData d = new(tex.Value, pos - Main.screenPosition, tex.Frame(), colour, 0f, tex.Size() / 2f, 28f / MathHelper.Max(tex.Width(), tex.Height()), SpriteEffects.None, 0);
						drawInfo.DrawDataCache.AddRange(a, d);
					}
				}
			}
		}
	}
}
