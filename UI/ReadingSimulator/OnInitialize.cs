using Terraria.GameContent.UI.Elements;
using Terraria.UI;

using static ArcaneOdyssey.UI._BaseImbueUI.BaseImbueUI;

namespace ArcaneOdyssey.UI.ReadingSimulator;

// Spoky (2026 Apr 08): If this isn't deleted after the UI is done, then I forgot to delete this
public abstract partial class ReadingSimulatorUI : UIState
{
	#region UI Panel declarations and some stuff related to it
	/// <summary>
	/// Main <see cref="UIPanel"/>, here every other panel will be placed on top of
	/// </summary>
	protected UIPanel main = new();

	/// <summary>
	/// Used for closing this <see cref="ReadingSimulatorUI"/>
	/// </summary>
	protected UIImageButton CloseButton = new(ButtonTextures.Neutral);
	/// <inheritdoc cref="CloseButton"/>
	protected UIText CloseText = new("Close", 1, true);
	#endregion

	#region Initialize (real)
	public override void OnInitialize()
	{
		#region Main Panel, the panel that serves  
		main.SetPadding(0);
		main.BackgroundColor = new(73, 94, 171);

		main.Width.Set(512f, 0f);
		main.Height.Set(512f, 0f);

		main.HAlign = 0.5f; main.VAlign = 0.2f;

		Append(main);
		#endregion
	}
	#endregion
}
