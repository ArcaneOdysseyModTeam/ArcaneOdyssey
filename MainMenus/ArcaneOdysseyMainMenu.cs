using ArcaneOdysseyMusic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArcaneOdyssey.MainMenus
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

			public static Asset<Texture2D> Texture => ArcaneOdysseyMod.Instance.Assets.Request<Texture2D>("Assets/Raindrop");

			public void Update()
			{
				lifevalue++;
				position += velocity;
				if (position.Y >= Main.screenHeight * 1.1f)
				{
					lifevalue = maxlife;
				}
			}

			public void Draw()
			{
				Main.spriteBatch.Draw(Texture.Value, position, SelectedTitle.Colour);
			}

			public Raindrop()
			{
				depth = Main.rand.NextFloat(1, .25f);
				maxlife = 120;//(int)Math.Round(100f * depth);
				velocity = new Vector2(0, 20) * depth;
				maxlife = (int)Math.Round(100f * depth);
				velocity = new Vector2(0, 20) * depth;
				position = new Vector2(Main.screenWidth * Main.rand.NextFloat(0f, 1f), 0 - Texture.Height());
			}
		}

		public struct MainMenuStyle
		{
			public MusicTrack Track;

			public Color Colour;

			public string Name;

			public LocalizedText DisplayName;

			public MainMenuStyle(MusicTrack track, Color colour, string name, Mod mod = null)
			{
				Track = track;
				Colour = colour;
				Name = name;
				mod ??= ArcaneOdysseyMod.Instance;
				DisplayName = mod.CoolCustomLocalization("MainMenuStyle." + Name);
			}
		}

		public static List<Raindrop> Raindrops = [];

		public Texture2D BackgroundTexture => Mod.Assets.Request<Texture2D>("MainMenus/Images/" + SelectedTitle.Name).Value;

		public override string DisplayName => Mod.CustomLocalization("MenuStyle", SelectedTitle.DisplayName.Value).Value;

		public override Asset<Texture2D> Logo => Mod.Assets.Request<Texture2D>("Assets/TitleLogo");

		public override int Music => SelectedTitle.Track.MusicSlot;

		public override void Update(bool isOnTitleScreen)
		{
			Main.time = 27000.0;
			Main.dayTime = true;
		}

		public override void SetStaticDefaults()
		{
			Titles.AddRange([new(MusicTrack.TitleTheme2, Color.White, "Classic"), new(MusicTrack.TitleTheme, Color.Transparent, "Pixel"), new(MusicTrack.DarkSea, Color.Gray, "Dragon"),]);
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

			spriteBatch.Draw(BackgroundTexture, drawOffset, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

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
				drop.Draw();
			}

			spriteBatch.End();

			logoRotation = 0f;
			logoScale = 1f;
			drawColor = Color.White;
			logoDrawCenter.Y = Logo.Height() / 2f;
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
			return true;
		}

		public static MainMenuStyle SelectedTitle;
		public static List<MainMenuStyle> Titles = [];

		public override void OnSelected()
		{
			SelectedTitle = Main.rand.Next(Titles);
		}
	}
}