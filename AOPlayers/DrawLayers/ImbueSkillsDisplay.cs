using ArcaneOdyssey.Imbues.Base;
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
		public override Position GetDefaultPosition() => PlayerDrawLayers.AfterLastVanillaLayer;

		public static Asset<Texture2D> backgroundSprite;
		public override void Load()
		{
			backgroundSprite = Mod.Assets.Request<Texture2D>("Assets/GelBuffBackground");
		}

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
					List<DrawData> drawDatas = [];
					Vector2 startingPos;

					if (ExternalModSupport.HasFargos)
					{
						startingPos = drawInfo.Position + new Vector2(Player.defaultWidth / 2f, -64f) - Main.screenPosition;
					}
					else
					{
						startingPos = drawInfo.Position + new Vector2(Player.defaultWidth / 2f, -32f) - Main.screenPosition;
					}

					var count = 0;
					if (imbue.Passive is not null)
						count++;
					if (imbue.Mobility is not null)
						count++;
					if (imbue.Dash is not null)
						count++;

					var secondaryItemPosX = startingPos.X - (32f * (count - 1f) / 2f);

					if (imbue.Passive is not null)
					{
						var pos = startingPos with { X = secondaryItemPosX };
						
						var colour = Color.White;
						if (!imbue.PassiveActive)
						{
							colour *= .5f;
						}	

						Texture2D texture = backgroundSprite.Value;
						DrawData a = new(texture, pos, texture.Frame(), colour, 0f, texture.Size() / 2f, 1f, SpriteEffects.None, 0);

						Asset<Texture2D> tex;
						if (imbue.Passive.Scroll != 0)
							tex = TextureAssets.Item[imbue.Passive.Scroll];
						else
							tex = TextureAssets.Item[imbue.Type];

						DrawData d = new(tex.Value, pos, tex.Frame(), colour, 0f, tex.Size() / 2f, 28f / MathHelper.Max(tex.Width(), tex.Height()), SpriteEffects.None, 0);
						drawDatas.AddRange(a, d);
						secondaryItemPosX += 32f;
					}
					if (imbue.Mobility is not null)
					{
						var pos = startingPos with { X = secondaryItemPosX };

						var colour = Color.White * .5f;

						Texture2D texture = backgroundSprite.Value;
						DrawData a = new(texture, pos, texture.Frame(), colour, 0f, texture.Size() / 2f, 1f, SpriteEffects.None, 0);

						Asset<Texture2D> tex;
						if (imbue.Mobility.Scroll != 0)
							tex = TextureAssets.Item[imbue.Mobility.Scroll];
						else
							tex = TextureAssets.Item[imbue.Type];

						DrawData d = new(tex.Value, pos, tex.Frame(), colour, 0f, tex.Size() / 2f, 28f / MathHelper.Max(tex.Width(), tex.Height()), SpriteEffects.None, 0);
						drawDatas.AddRange(a, d);
						secondaryItemPosX += 32f;
					}
					if (imbue.Dash is not null)
					{
						var pos = startingPos with { X = secondaryItemPosX };

						var colour = Color.White * .5f;

						Texture2D texture = backgroundSprite.Value;
						DrawData a = new(texture, pos, texture.Frame(), colour, 0f, texture.Size() / 2f, 1f, SpriteEffects.None, 0);

						Asset<Texture2D> tex;
						if (imbue.Dash.Scroll != 0)
							tex = TextureAssets.Item[imbue.Dash.Scroll];
						else
							tex = TextureAssets.Item[imbue.Type];

						DrawData d = new(tex.Value, pos, tex.Frame(), colour, 0f, tex.Size() / 2f, 28f / MathHelper.Max(tex.Width(), tex.Height()), SpriteEffects.None, 0);
						drawDatas.AddRange(a, d);
						secondaryItemPosX += 32f;
					}

					if (count > 0)
						startingPos.Y -= 32f;

					for (int i = 0; i < 3; i++)
					{
						var pos = startingPos;
						switch (i)
						{
							case 0:
								pos.X -= 32f;
								break;
							case 2:
								pos.X += 32f;
								break;
						}
						var spell = imbue.Skills[i];
						if (spell is not null)
						{
							var colour = Color.White;
							if (imbue.selectedIndex != i)
							{
								colour *= .5f;
							}

							Texture2D texture = backgroundSprite.Value;
							DrawData a = new(texture, pos, texture.Frame(), colour, 0f, texture.Size() / 2f, 1f, SpriteEffects.None, 0);

							Asset<Texture2D> tex;
							if (spell.Scroll != 0)
								tex = TextureAssets.Item[spell.Scroll];
							else
								tex = TextureAssets.Item[imbue.Type];

							DrawData d = new(tex.Value, pos, tex.Frame(), colour, 0f, tex.Size() / 2f, 28f / MathHelper.Max(tex.Width(), tex.Height()), SpriteEffects.None, 0);
							drawDatas.AddRange(a, d);
						}
					}
					
					drawInfo.DrawDataCache.AddRange(drawDatas);
				}
			}
		}
	}
}
