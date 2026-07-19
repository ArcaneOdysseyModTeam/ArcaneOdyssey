using ArcaneOdysseyMusic;
using System;
using System.Collections.Generic;

namespace ArcaneOdyssey.MainMenu
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

			public static Asset<Texture2D> Texture = Asset<Texture2D>.Empty;

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
				Main.spriteBatch.Draw(Texture.Value, position, null, SelectedTitle.Colour, 0, Texture.Size() / 2f, ((3f * depth) - (2f * depth.Pow())) / 2f, SpriteEffects.None, 0f);
			}

			public Raindrop()
			{
				depth = Main.rand.NextFloat(1, .25f);
				maxlife = (int)Math.Round(100f * depth);
				velocity = new Vector2(0, 20) * depth;
				position = new Vector2(Main.screenWidth * Main.rand.NextFloat(0f, 1f), 0 - Texture.Height() - velocity.Y);
			}
		}

		public struct MainMenuStyle
		{
			public MusicTrack Track;

			public Color Colour;

			public string Name;

			public LocalizedText DisplayName;
			public Asset<Texture2D> BackgroundTexture;

			public MainMenuStyle(MusicTrack track, Color colour, string name, Mod mod = null, string path = "MainMenu/Images")
			{
				Track = track;
				Colour = colour;
				Name = name;
				mod ??= ArcaneOdysseyMod.Instance;
				DisplayName = mod.CoolCustomLocalization($"MainMenuStyle.{Name}");
				BackgroundTexture = AOUtils.Request($"{mod.Name}/{path}/{Name}", ref BackgroundTexture, AssetRequestMode.ImmediateLoad);
			}
		}

		public static List<Raindrop> Raindrops = [];

		public override string DisplayName => Mod.CustomLocalization("MenuStyle", SelectedTitle.DisplayName.Value).Value;

		private static Asset<Texture2D> cachedLogo;
		public override Asset<Texture2D> Logo => AOUtils.Request(Mod.Name + "/Assets/TitleLogo", ref cachedLogo);

		public override int Music => SelectedTitle.Track.MusicSlot;

		public override void Update(bool isOnTitleScreen)
		{
			Main.time = 27000.0;
			Main.dayTime = true;
		}

		public override void SetStaticDefaults()
		{
			Raindrop.Texture = Mod.Assets.Request<Texture2D>("Assets/Raindrop");
			Titles.AddRange(new(MusicTrack.TitleTheme2, Color.White, "Classic"), new(MusicTrack.TitleTheme, Color.Transparent, "Pixel"), new(MusicTrack.DarkSea, Color.Gray, "Dragon"), new(MusicTrack.Djin, Color.Gray, "Djin"));
		}

		public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
		{
			Vector2 drawOffset = Vector2.Zero;
			float xScale = (float)Main.screenWidth / SelectedTitle.BackgroundTexture.Width();
			float yScale = (float)Main.screenHeight / SelectedTitle.BackgroundTexture.Height();
			float scale = xScale;

			if (xScale != yScale)
			{
				if (yScale > xScale)
				{
					scale = yScale;
					drawOffset.X -= (SelectedTitle.BackgroundTexture.Width() * scale - Main.screenWidth) * 0.5f;
				}
				else
					drawOffset.Y -= (SelectedTitle.BackgroundTexture.Height() * scale - Main.screenHeight) * 0.5f;
			}

			spriteBatch.Draw(SelectedTitle.BackgroundTexture.Value, drawOffset, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

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
			logoScale = .85f;
			drawColor = Color.White;
			logoDrawCenter.Y = Logo.Height() / 2f * logoScale;
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
			return true;
		}

		internal static MainMenuStyle SelectedTitle;
		public static List<MainMenuStyle> Titles = [];

		public override void OnSelected()
		{
			SelectedTitle = Main.rand.Next(Titles);
		}
	}
}