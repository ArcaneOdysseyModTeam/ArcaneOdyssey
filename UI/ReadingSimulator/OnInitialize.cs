using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace ArcaneOdyssey.UI.ReadingSimulator;

// Spoky (2026 Apr 08): If this isn't deleted after the UI is done, then I forgot to delete this
public partial class ReadingSimulatorUI : UIState
{
	#region UI Panel declarations and some stuff related to it
	/// <summary>
	/// Main <see cref="UIPanel"/>, here every other panel will be placed on top of this one
	/// </summary>
	protected UIPanel main = new();

	/// <summary>
	/// Used for closing this <see cref="ReadingSimulatorUI"/>
	/// </summary>
	protected UIImageButton CloseButton = new(ButtonTextures.Close);

	#region Textures
	public class ButtonTextures
	{
		public static readonly Asset<Texture2D> Close = ArcaneOdysseyMod.Instance.Assets.Request<Texture2D>("UI/SharedImages/CloseButton", AssetRequestMode.ImmediateLoad);
	}
	#endregion
	#endregion

	#region Initialize (real)
	public override void OnInitialize()
	{
		#region Main Panel, the panel that serves the main function
		main.SetPadding(0);
		main.BackgroundColor = new(73, 94, 171);

		main.Width.Set(512f, 0f);
		main.Height.Set(512f, 0f);

		main.HAlign = 0.5f; main.VAlign = 0.4f;

		Append(main);
		#endregion

		#region Close button
		CloseButton.Width.Set(32f, 0f);
		CloseButton.Height.Set(32f, 0f);

		CloseButton.Left.Set(4f, 0f);
		CloseButton.Top.Set(4f, 0f);

		CloseButton.OnLeftClick += CloseButton_OnLeftClick;

		main.Append(CloseButton);
		#endregion
	}
	#endregion
}
