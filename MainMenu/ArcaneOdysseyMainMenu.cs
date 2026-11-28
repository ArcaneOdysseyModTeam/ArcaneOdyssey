using ArcaneOdysseyMusic;
using System;
using System.Collections.Generic;
using Terraria.GameContent.Events;
using static ArcaneOdyssey.MainMenu.ArcaneOdysseyMainMenu;

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
			public MainMenuStyle Style { get; }

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
				Main.spriteBatch.Draw(Texture.Value, position, null, Style.Colour, 0, Texture.Size() / 2f, ((3f * depth) - (2f * depth.Pow())) / 2f, SpriteEffects.None, 0f);
			}

			public Raindrop(MainMenuStyle style)
			{
				Style = style;
				depth = Main.rand.NextFloat(1, .25f);
				maxlife = (int)Math.Round(100f * depth);
				velocity = new Vector2(0, 20) * depth;
				position = new Vector2(Main.screenWidth * Main.rand.NextFloat(0f, 1f), 0 - Texture.Height() - velocity.Y);
			}
		}

		public struct MainMenuStyle
		{
			public MusicTrack Track { get; }

			public Color Colour { get; }

			public string Name { get; }

			public readonly LocalizedText DisplayName => Mod.CoolCustomLocalization($"MainMenuStyle.{Name}");
			public Asset<Texture2D> BackgroundTexture;

			public Mod Mod { get; }

			public Func<bool> Requirement { get; }

			public MainMenuStyle(MusicTrack track, Color colour, string name, Func<bool> condition = null, Mod mod = null, string path = "MainMenu/Images")
			{
				Track = track;
				Colour = colour;
				Name = name;
				Mod = mod ?? ArcaneOdysseyMod.Instance;
				Requirement = condition;
				BackgroundTexture = AOUtils.Request($"{Mod.Name}/{path}/{Name}", ref BackgroundTexture, AssetRequestMode.ImmediateLoad);
			}
		}

		public static List<Raindrop> Raindrops = [];

		public override string DisplayName => Mod.CustomLocalization("MenuStyle", SelectedTitle.DisplayName.Value).Value;

		internal static Asset<Texture2D> cachedLogo;
		public override Asset<Texture2D> Logo => AOUtils.Request(Mod.Name + "/Assets/TitleLogo", ref cachedLogo);

		public override int Music => SelectedTitle.Track.MusicSlot;

		public override bool IsAvailable => ArcaneOdysseyClientConfig.Instance.RandomMenuStyle && Titles.Count > 0;

		public override void Update(bool isOnTitleScreen)
		{
			Main.time = 27000.0;
			Main.dayTime = true;
		}

		public override void SetStaticDefaults()
		{
			foreach (var title in Titles)
			{
				_ = title.DisplayName;
			}
		}

		internal static void DrawAOBackground(SpriteBatch spriteBatch)
		{
			if (SelectedTitle.Name.IsNullOrWhiteSpace())
			{
				RandomSelect();
			}
			if (!CreditsRollEvent.IsEventOngoing)
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
			}
			if (SelectedTitle.Colour != Color.Transparent)
			{
				spriteBatch.End();

				if (Raindrops.Count <= 800)
				{
					Raindrops.Add(new(SelectedTitle));
				}

				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);

				Raindrops.RemoveAll(e => e.lifevalue >= e.maxlife);

				foreach (Raindrop drop in Raindrops)
				{
					drop.Update();
					drop.Draw();
				}
			}
		}

		public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
		{
			DrawAOBackground(spriteBatch);
			spriteBatch.End();

			logoRotation = 0f;
			logoScale = .85f;
			drawColor = Color.White;
			logoDrawCenter.Y = Logo.Height() / 2f * logoScale;
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
			return true;
		}

		internal static MainMenuStyle SelectedTitle;

		/// <summary>
		/// Must be added to *IN* <seealso cref="Mod.Load"/>
		/// </summary>
		public static List<MainMenuStyle> Titles = [];

		public override void OnSelected()
		{
			RandomSelect();
		}

		public override void Load()
		{
			Raindrop.Texture = Mod.Assets.Request<Texture2D>("Assets/Raindrop");
		}

		public static void RandomSelect()
		{
			SelectedTitle = Main.rand.Next(Titles.FindAll(e => e.Requirement is null || e.Requirement()));
		}

		public override bool IsLoadingEnabled(Mod mod) => !Main.dedServ;
	}

	public class MainMenuLoader : ModSystem
	{
		public override void OnModLoad()
		{
			if (!Main.dedServ)
			{
				foreach (var title in Titles)
				{
					title.Mod.AddContent(new NonRandomMenu(title));
				}
			}
		}
	}

	[Autoload(false)]
	public class NonRandomMenu(MainMenuStyle Style) : ModMenu
	{
		private MainMenuStyle SelectedTitle = Style;
		public override string Name => SelectedTitle.Name;
		public override string DisplayName => Mod.CustomLocalization("MenuStyle", SelectedTitle.DisplayName.Value).Value;
		public override bool IsLoadingEnabled(Mod mod) => !(Main.dedServ || SelectedTitle.Name.IsNullOrWhiteSpace());

		public override bool IsAvailable => !ArcaneOdysseyClientConfig.Instance.RandomMenuStyle && (SelectedTitle.Requirement is null || SelectedTitle.Requirement());
		public override Asset<Texture2D> Logo => AOUtils.Request(Mod.Name + "/Assets/TitleLogo", ref cachedLogo);

		private void DrawAOBackground(SpriteBatch spriteBatch)
		{
			if (!CreditsRollEvent.IsEventOngoing)
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
			}

			if (SelectedTitle.Colour != Color.Transparent)
			{

				spriteBatch.End();

				if (Raindrops.Count <= 800)
				{
					Raindrops.Add(new(SelectedTitle));
				}

				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);

				Raindrops.RemoveAll(e => e.lifevalue >= e.maxlife);

				foreach (Raindrop drop in Raindrops)
				{
					drop.Update();
					drop.Draw();
				}
			}
		}

		public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
		{
			DrawAOBackground(spriteBatch);
			spriteBatch.End();

			logoRotation = 0f;
			logoScale = .85f;
			drawColor = Color.White;
			logoDrawCenter.Y = Logo.Height() / 2f * logoScale;
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
			return true;
		}

		public override void Update(bool isOnTitleScreen)
		{
			Main.time = 27000.0;
			Main.dayTime = true;
		}
		public override int Music => SelectedTitle.Track.MusicSlot;
	}
}