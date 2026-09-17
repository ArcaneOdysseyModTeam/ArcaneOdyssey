using ArcaneOdyssey.MainMenu;
using ReLogic.Graphics;
using SubworldLibrary;
using Terraria.GameContent;
using Terraria.GameInput;

namespace ArcaneOdyssey.Subworlds.Base
{
	public abstract class AOSubworld : Subworld
	{
		public const int MaxSeed = 2147483647;
		public override void DrawMenu(GameTime gameTime)
		{
			Main.spriteBatch.DrawString(FontAssets.DeathText.Value, message, new Vector2(Main.screenWidth, Main.screenHeight) / 2f - FontAssets.DeathText.Value.MeasureString(message) / 2f, Color.White);
		}

		public override void DrawSetup(GameTime gameTime)
		{
			PlayerInput.SetZoom_UI();
			Main.instance.GraphicsDevice.Clear(Color.Black);
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
			ArcaneOdysseyMainMenu.DrawAOBackground(Main.spriteBatch);
			DrawMenu(gameTime);
			//Main.DrawCursor(Main.DrawThickCursor());
			Main.spriteBatch.End();
		}

		public static string message = string.Empty;

		public override void OnEnter()
		{
			SubworldSystem.noReturn = true;
			SubworldSystem.hideUnderworld = true;
			message = $"Traveling to {DisplayName}...";
		}

		public override void OnExit()
		{
			message = "Returning...";
		}
	}
}
