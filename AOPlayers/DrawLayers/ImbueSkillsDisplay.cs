using ArcaneOdyssey.Imbues.Base;
using Fargowiltas;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace ArcaneOdyssey.AOPlayers.DrawLayers
{
	public class ImbueSkillsDisplay : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new Between();

		public static (Asset<Texture2D>, Asset<Texture2D>) backgroundSprites;
		public static Rectangle dimensions;
		public static float spriteSize;
		public override void Load()
		{
			backgroundSprites = (Mod.Assets.Request<Texture2D>("Assets/SelectedScrollIcon"), Mod.Assets.Request<Texture2D>("Assets/UnsellectedScrollIcon"));
		}

		public override void SetStaticDefaults()
		{
			dimensions = backgroundSprites.Item1.Frame();
			spriteSize = dimensions.Width - 4f;
		}

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			return !Main.hideUI
			&& drawInfo.drawPlayer.whoAmI == Main.myPlayer
			&& drawInfo.drawPlayer.active
			&& !drawInfo.drawPlayer.DeadOrGhost
			&& drawInfo.shadow == 0
			&& !Main.gameMenu;
		}

		[JITWhenModsEnabled("Fargowiltas")]
		public bool FargosBuffDisplayActive(PlayerDrawSet drawInfo) => ModContent.GetInstance<FargoPlayerBuffDrawLayer>().GetDefaultVisibility(drawInfo);

		public override bool IsHeadLayer => false;

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player player = drawInfo.drawPlayer;

			if (player.PlayerItem().ModItem is Imbuable imbue)
			{
				List<DrawData> drawDatas = [];
				Vector2 startingPos;

				if (ExternalModSupport.HasFargos && FargosBuffDisplayActive(drawInfo))
				{
					startingPos = drawInfo.Position + new Vector2(Player.defaultWidth / 2f, -32f - dimensions.Height) - Main.screenPosition;
				}
				else
				{
					startingPos = drawInfo.Position + new Vector2(Player.defaultWidth / 2f, -dimensions.Height) - Main.screenPosition;
				}

				var count = 0;
				if (imbue.Passive is not null)
					count++;
				if (imbue.Mobility is not null)
					count++;
				if (imbue.Dash is not null)
					count++;

				var secondaryItemPosX = startingPos.X - (dimensions.Width * (count - 1f) / 2f);

				if (imbue.Passive is not null)
				{
					var pos = startingPos with { X = secondaryItemPosX };
					var texture = backgroundSprites.Item1.Value;
						
					var colour = Color.White;
					if (!imbue.PassiveActive)
					{
						colour *= .75f;
						texture = backgroundSprites.Item2.Value;
					}
					DrawData a = new(texture, pos, texture.Frame(), colour, 0f, texture.Size() / 2f, 1f, SpriteEffects.None, 0);

					Asset<Texture2D> tex;
					if (imbue.Passive.Scroll != 0)
						tex = TextureAssets.Item[imbue.Passive.Scroll];
					else
						tex = TextureAssets.Item[imbue.Type];

					DrawData d = new(tex.Value, pos, tex.Frame(), colour, 0f, tex.Size() / 2f, spriteSize / MathHelper.Max(tex.Width(), tex.Height()), SpriteEffects.None, 0);
					drawDatas.AddRange(a, d);
					secondaryItemPosX += dimensions.Width;
				}
				if (imbue.Mobility is not null)
				{
					var pos = startingPos with { X = secondaryItemPosX };

					var colour = Color.White * .75f;

					Texture2D texture = backgroundSprites.Item2.Value;
					DrawData a = new(texture, pos, texture.Frame(), colour, 0f, texture.Size() / 2f, 1f, SpriteEffects.None, 0);

					Asset<Texture2D> tex;
					if (imbue.Mobility.Scroll != 0)
						tex = TextureAssets.Item[imbue.Mobility.Scroll];
					else
						tex = TextureAssets.Item[imbue.Type];

					DrawData d = new(tex.Value, pos, tex.Frame(), colour, 0f, tex.Size() / 2f, spriteSize / MathHelper.Max(tex.Width(), tex.Height()), SpriteEffects.None, 0);
					drawDatas.AddRange(a, d);
					secondaryItemPosX += dimensions.Width;
				}
				if (imbue.Dash is not null)
				{
					var pos = startingPos with { X = secondaryItemPosX };

					var colour = Color.White * .75f;

					Texture2D texture = backgroundSprites.Item2.Value;
					DrawData a = new(texture, pos, texture.Frame(), colour, 0f, texture.Size() / 2f, 1f, SpriteEffects.None, 0);

					Asset<Texture2D> tex;
					if (imbue.Dash.Scroll != 0)
						tex = TextureAssets.Item[imbue.Dash.Scroll];
					else
						tex = TextureAssets.Item[imbue.Type];

					DrawData d = new(tex.Value, pos, tex.Frame(), colour, 0f, tex.Size() / 2f, spriteSize / MathHelper.Max(tex.Width(), tex.Height()), SpriteEffects.None, 0);
					drawDatas.AddRange(a, d);
					secondaryItemPosX += dimensions.Width;
				}

				if (count > 0)
					startingPos.Y -= dimensions.Height;

				for (int i = 0; i < 3; i++)
				{
					var pos = startingPos;
					switch (i)
					{
						case 0:
							pos.X -= dimensions.Width;
							break;
						case 2:
							pos.X += dimensions.Width;
							break;
					}
					var spell = imbue.Skills[i];
					if (spell is not null)
					{
						var colour = Color.White;

						Texture2D texture = backgroundSprites.Item1.Value;

						if (imbue.selectedIndex != i)
						{
							colour *= .75f;
							texture = backgroundSprites.Item2.Value;
						}

						DrawData a = new(texture, pos, texture.Frame(), colour, 0f, texture.Size() / 2f, 1f, SpriteEffects.None, 0);

						Asset<Texture2D> tex;
						if (spell.Scroll != 0)
							tex = TextureAssets.Item[spell.Scroll];
						else
							tex = TextureAssets.Item[imbue.Type];

						DrawData d = new(tex.Value, pos, tex.Frame(), colour, 0f, tex.Size() / 2f, (MathHelper.Min(dimensions.Height, dimensions.Width) - 4f) / MathHelper.Max(tex.Width(), tex.Height()), SpriteEffects.None, 0);
						drawDatas.AddRange(a, d);
					}
				}
					
				drawInfo.DrawDataCache.AddRange(drawDatas);
			}
		}
	}
}

