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
		public class Raindrop
		{
			public int maxlife;
			public int lifevalue;
			public float depth;
			public Vector2 velocity;
			public Vector2 position;

			public static Asset<Texture2D> Texture => ModContent.Request<Texture2D>("ArcaneOdyssey/Title/Raindrop");

			public void Update()
			{
				lifevalue++;
				position += velocity;
				if (position.Y >= Main.screenHeight * 1.1f)
				{
					lifevalue = maxlife;
				}
			}

			public void Draw(bool dark)
			{
				Main.spriteBatch.Draw(Texture.Value, position, !dark ? new Color(255f, 255f, 255f, 255f/10f) : Color.Black);
            }

			public Raindrop()
            {
                depth = Main.rand.NextFloat(1, .25f);
                maxlife = 120;//(int)Math.Round(100f * depth);
                velocity = new Vector2(0, 20) * (depth);
                maxlife = (int)Math.Round(100f * depth);
                velocity = new Vector2(0, 20) * depth;
				position = new Vector2(Main.screenWidth * Main.rand.NextFloat(0f, 1f), 0);
			}
		}

		public static List<Raindrop> Raindrops = [];

		public static Texture2D BackgroundTexture => ModContent.Request<Texture2D>("ArcaneOdyssey/Title/TitleBackground").Value;

		public override string DisplayName => !AltMenu ? Mod.CustomLocalization("MenuStyle").Value : Mod.CustomLocalization("AltMenuStyle").Value;

		public override ModSurfaceBackgroundStyle MenuBackgroundStyle => ModContent.GetInstance<TheTitleStyle>();

		public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>("ArcaneOdyssey/Assets/TitleLogo");

		public override Asset<Texture2D> MoonTexture => ModContent.Request<Texture2D>("ArcaneOdyssey/Backgrounds/Blank");

		public override Asset<Texture2D> SunTexture => ModContent.Request<Texture2D>("ArcaneOdyssey/Backgrounds/Blank");

		public override int Music => GetMusic();

		public int GetMusic()
		{
			int mus = MusicID.OtherworldlyRain;
			if (!AltMenu)
			{
				mus = MusicLoader.GetMusicSlot(Mod, "Music/TitleTheme");
            }
			else
			{
				mus = MusicLoader.GetMusicSlot(Mod, "Music/DarkSea");
			}
			return mus != 0 ? mus : MusicID.OtherworldlyRain;
        }

		/// <summary>
		/// dark sea menu 
		/// </summary>
		public virtual bool AltMenu => false;

		public override void Update(bool isOnTitleScreen)
		{
			Main.time = 27000.0;
			Main.dayTime = !AltMenu;
		}
		public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
		{
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

			var thecolour = AltMenu ? Color.Gray : Color.White;

            spriteBatch.Draw(BackgroundTexture, drawOffset, null, thecolour, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

			Main.time = 27000;
			Main.dayTime = !AltMenu;

			spriteBatch.End();


			if (Raindrops.Count <= 800)
			{
				Raindrops.Add(new());
			}

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);

			Raindrops.RemoveAll(e => e.lifevalue >= e.maxlife);

            foreach (Raindrop drop in Raindrops)
			{
				drop.Update();
                drop.Draw(AltMenu);
			}

			spriteBatch.End();

			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
			spriteBatch.Draw(Logo.Value, new(Main.screenWidth/2f, 100f), null, thecolour, 0, Logo.Value.Size()/2f, 1f, SpriteEffects.None, 0f);
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

		public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b) => BackgroundTextureLoader.GetBackgroundSlot("ArcaneOdyssey/Backgrounds/Blank");
		public override int ChooseFarTexture() => BackgroundTextureLoader.GetBackgroundSlot("ArcaneOdyssey/Backgrounds/Blank");
		public override int ChooseMiddleTexture() => BackgroundTextureLoader.GetBackgroundSlot("ArcaneOdyssey/Backgrounds/Blank");
		public override bool PreDrawCloseBackground(SpriteBatch spriteBatch) => false;
	}

	public class DarkTitle : ArcaneOdysseyMainMenu
	{
        public override bool AltMenu => true;
	}
}