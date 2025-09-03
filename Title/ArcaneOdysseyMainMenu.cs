using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.RGB;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Title
{
	public class ArcaneOdysseyMainMenu : ModMenu
	{
		/// <summary>
		/// A RAINDROP WHY IS IT SO COMPLICATED TO DO
		/// </summary>
		/// <param name="number">the number of this raindrop, aka Raindrops.Count</param>
		/// <param name="depth">how far back this is, 0.25 is all the way back while 1 is at the front</param>
		/// <param name="position">starting position of this</param>
		/// <param name="velocity">velocity of the rain</param>
		public class Raindrop
		{
			public int maxlife;
			public int lifevalue;
			public float depth;
			public Vector2 velocity;
			public Vector2 position;

			public static Asset<Texture2D> Texture => ModContent.Request<Texture2D>("ArcaneOdyssey/Title/Raindrop");

			/// <summary>
			/// somehow call every frame idk
			/// </summary>
			public void Update()
			{
				lifevalue += 1;
				position += velocity;
			}

			public void Draw()
			{
				Main.spriteBatch.Draw(Texture.Value, position, new Color(255f, 255f, 255f, 255f/4f));
            }

			public Raindrop()
            {
                depth = Main.rand.NextFloat(1, .25f);
                maxlife = (int)Math.Round(60f * depth);
                velocity = new Vector2(0, 20) * depth;
				position = new Vector2(Main.screenWidth * Main.rand.NextFloat(0f, 1f), 0);
			}
		}

		public static List<Raindrop> Raindrops = [];

		public static Texture2D BackgroundTexture => ModContent.Request<Texture2D>("ArcaneOdyssey/Title/TitleBackground").Value;

		public override string DisplayName => Mod.CustomLocalization("MenuStyle").Value;

		public override ModSurfaceBackgroundStyle MenuBackgroundStyle => ModContent.GetInstance<TheTitleStyle>();

		// public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>("ArcaneOdyssey/ExtraAssets/Blank"); add logo here later

		public override Asset<Texture2D> MoonTexture => ModContent.Request<Texture2D>("ArcaneOdyssey/Title/Blank");

		public override Asset<Texture2D> SunTexture => ModContent.Request<Texture2D>("ArcaneOdyssey/Title/Blank");

		public override int Music => MusicID.OtherworldlyRain;

		/// <summary>
		/// if we add a dark sea menu or something idk, unused
		/// </summary>
		public virtual bool AltMenu => false;

		public override void Update(bool isOnTitleScreen)
		{
			Main.time = 27000.0;
			Main.dayTime = true;
		}
		public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
		{
			// you all have NO CLUE HOW LONG THIS TOOK TO DO, STUDYING HOW OTHER MODS DO THISAHHHHHHHHHHHHHHHHh

			Vector2 drawOffset = Vector2.Zero;
			float xScale = (float)Main.screenWidth / BackgroundTexture.Width;
			float yScale = (float)Main.screenHeight / BackgroundTexture.Height;
			float scale = xScale;

			if (xScale != yScale)
			{
				if (yScale > xScale)
				{
					scale = yScale;
					drawOffset.X -= (BackgroundTexture.Width * scale - Main.screenWidth) * 0.5f;
				}
				else
					drawOffset.Y -= (BackgroundTexture.Height * scale - Main.screenHeight) * 0.5f;
			}


			spriteBatch.Draw(BackgroundTexture, drawOffset, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

			Main.time = 27000;
			Main.dayTime = true;

			spriteBatch.End();


			if (Raindrops.Count <= 600)
			{
				Raindrops.Add(new());
			}

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);

			Raindrops.RemoveAll(e => e.lifevalue >= e.maxlife);

            foreach (Raindrop drop in Raindrops)
			{
				drop.Update();
                drop.Draw();
			}

			spriteBatch.End();

			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
			spriteBatch.Draw(Logo.Value, new(Main.screenWidth / 2f, 100f), null, Color.White, 0, Logo.Value.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
			return false;
		}
	}

	public class TheTitleStyle : ModSurfaceBackgroundStyle
	{
		public override void ModifyFarFades(float[] fades, float transitionSpeed)
		{
			for (int i = 0; i < fades.Length; i++)
			{
				if (i == Slot)
				{
					fades[i] = 1f;
				}
				else
				{
					fades[i] = 0f;
				}
			}
		}

		public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b) => BackgroundTextureLoader.GetBackgroundSlot("ArcaneOdyssey/Title/Blank");
		public override int ChooseFarTexture() => BackgroundTextureLoader.GetBackgroundSlot("ArcaneOdyssey/Title/Blank");
		public override int ChooseMiddleTexture() => BackgroundTextureLoader.GetBackgroundSlot("ArcaneOdyssey/Title/Blank");
		public override bool PreDrawCloseBackground(SpriteBatch spriteBatch) => false;
	}
}