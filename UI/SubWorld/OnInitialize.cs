using ArcaneOdyssey.Subworlds.Base;
using ArcaneOdyssey.UI.ReadingSimulator;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace ArcaneOdyssey.UI.SubWorld;

public partial class SubWorldUI : UIState
{
	// Spoky 2026 Sept 30: Stuff as ui panel declarations and OnInitialize will be here

	/// <summary>
	/// main <see cref="UIPanel"/>, the one that is in the background of everything
	/// </summary>
	public UIPanel main = new();

	protected UIText UnlockHint = new("") { IgnoresMouseInteraction = true };

	#region Subworld options
	public UIList SubworldOptions = [];
	public UIScrollbar SubworldScroller = new();
	#endregion

	#region Selected Subworld Panel
	/// <summary>
	/// Once a subworld is selected, its information (whether locked or unlocked) will appear here
	/// </summary>
	protected UIPanel SelectionPanel = new();

	/// <summary>
	/// The title of the selected <see cref="AOSubworld"/>
	/// </summary>
	protected UIText TitleSelection = new("", large: true);
	/// <summary>
	/// The image of the selected <see cref="AOSubworld"/>
	/// </summary>
	public UIImage TitleImage { get; protected set; } = new(
		ArcaneOdysseyMod.Instance.Assets.Request<Texture2D>("MainMenu/Images/Djin", AssetRequestMode.ImmediateLoad)
	);
	/// <summary>
	/// The description of the selected <see cref="AOSubworld"/>
	/// </summary>
	protected UIText TitleDescription = new("") { IgnoresMouseInteraction = true, IsWrapped = true };
	#endregion

	#region Other things, mainly stuff at the right
	protected UIImageButton CloseButton = new(ReadingSimulatorUI.ButtonTextures.Close);

	protected UIPanel EnterButton = new();
	/// <summary>
	/// The text of <see cref="EnterButton"/>
	/// </summary>
	protected UIText EnterText = new("Enter", large: true) { IgnoresMouseInteraction = true, IsWrapped = true }; 
	#endregion

	public override void OnInitialize()
	{
		float titlePlaceholder = 32f;

		#region Main Panel, the panel that serves the main function
		main.SetPadding(0);
		main.BackgroundColor = new(73, 94, 171);

		main.Width.Set(900f, 0f);
		main.Height.Set(640f, 0f);

		main.HAlign = 0.5f; main.VAlign = 0.4f;

		Append(main);
		#endregion

		#region Subworld Selection
		float top = separation + 8f,
			height = (main.Height.Pixels - (top + separation));

		#region Scroll bar declaration
		SubworldScroller.Width.Set(20f, 0f);
		// Spoky (2026 Apr 30): I have no idea why I have to do this, maybe UIScrollbar has a passive +~8 pixels of height?
		// Spoky (2026 Oct 07): Not going to question it, just going to copy and paste it
		SubworldScroller.Height.Set(height - 8f, 0f);
		SubworldScroller.Left.Set(separation, 0f);
		SubworldScroller.Top.Set(top, 0f);
		#endregion

		#region Scroll Area declaration
		SubworldOptions.Width.Set(164f + SubworldScroller.Width.Pixels, 0f);
		SubworldOptions.Height.Set(height, 0f);

		SubworldOptions.Left.Set(SubworldScroller.Left.Pixels + SubworldScroller.Width.Pixels + separation, 0f);
		SubworldOptions.Top.Set(top, 0f);
		#endregion

		for (int i = 0; i < AOSubworld.ALLSUBWORLDS.Length; i++)
		{
			var world = AOSubworld.ALLSUBWORLDS[i];
			if (world.ShowInUI)
			{
				var choice = new SubworldPicker(this, world, new(SubworldOptions.Width.Pixels - SubworldScroller.Width.Pixels, 32), top, i);
				SubworldOptions.Add(choice);
			}
		}

		main.Append(SubworldOptions);
		main.Append(SubworldScroller);

		SubworldOptions.SetScrollbar(SubworldScroller);
		#endregion

		#region Selected Panel

		#region Selection Main Panel
		SelectionPanel.SetPadding(0);
		SelectionPanel.BackgroundColor = new(50, 70, 130);

		SelectionPanel.Width.Set(DefaultImageSize.X + separation * 4, 0f);
		SelectionPanel.Height.Set(main.Height.Pixels - (separation * 2), 0f);

		// Spoky (2026 Oct 07): No idea, one separation should've been enough, checking if two are enough
		SelectionPanel.Left.Set(separation + SubworldOptions.Width.Pixels + (separation * 2), 0f);
		SelectionPanel.Top.Set(separation, 0f);

		main.Append(SelectionPanel);
		#endregion

		#region Subworld Title
		TitleSelection.Width.Set(DefaultImageSize.X + (separation * 2), 0f);
		TitleSelection.Height.Set(titlePlaceholder, 0f);

		TitleSelection.Left.Set(separation * 2, 0f);
		TitleSelection.Top.Set(separation * 4, 0f);

		SelectionPanel.Append(TitleSelection);
		#endregion

		#region Subworld Image
		ReSetTitleImageSize();
		TitleImage.Color = new Color(0, 0, 0, 0);
		SelectionPanel.Append(TitleImage);
		#endregion

		#region Subworld Description
		TitleDescription.Width.Set(TitleImage.Width.Pixels, 0f);
		TitleDescription.Height.Set(main.Height.Pixels - (TitleImage.Top.Pixels + TitleImage.Height.Pixels + separation * 4), 0f);

		TitleDescription.Left.Set(TitleImage.Left.Pixels, 0f);
		TitleDescription.Top.Set(TitleImage.Top.Pixels + TitleImage.Height.Pixels + separation * 2, 0f);

		SelectionPanel.Append(TitleDescription);
		#endregion
		#endregion

		#region Others Panel

		#region Close Button
		CloseButton.Width.Set(32f, 0f);
		CloseButton.Height.Set(32f, 0f);

		CloseButton.Left.Set(SelectionPanel.Left.Pixels + SelectionPanel.Width.Pixels + separation, 0f);
		CloseButton.Top.Set(separation, 0f);

		CloseButton.OnLeftClick += CloseButton_OnLeftClick;

		main.Append(CloseButton);
		#endregion

		#region Enter Button
		EnterButton.IgnoresMouseInteraction = true;
		EnterButton.BackgroundColor = new(50, 70, 130);

		EnterButton.Width.Set(main.Width.Pixels - (SelectionPanel.Left.Pixels + SelectionPanel.Width.Pixels + separation * 2), 0f);
		EnterButton.Height.Set(64, 0f);

		EnterButton.Left.Set(SelectionPanel.Left.Pixels + SelectionPanel.Width.Pixels + separation, 0f);
		EnterButton.Top.Set(main.Height.Pixels - (EnterButton.Height.Pixels + separation), 0f);

		EnterButton.OnLeftClick += EnterButton_OnLeftClick;

		main.Append(EnterButton);

		EnterText.SetText("");
		EnterText.Width.Set(EnterButton.Width.Pixels - separation * 2, 0f);
		EnterText.Height.Set(EnterButton.Height.Pixels - separation * 2, 0f);

		EnterText.IgnoresMouseInteraction = true;

		EnterText.HAlign = 0.5f;

		// Spoky (2026 February 03): VAlign for close text doesn't seem to work, but this does? ? ?
		// Spoky (2026 February 03): Alright nevermind, can't be bothered to make it perfectly centered
		// Spoky (2026 Oct 25): Deleted this, then got unpleasantly remembered as to why this was added in the first place
		EnterText.Top.Set(0, 0.15f);

		EnterButton.Append(EnterText);
		#endregion

		#endregion

		#region Others
		UnlockHint.Width.Set(32, 0);
		UnlockHint.Height.Set(32, 0);

		Append(UnlockHint);
		#endregion
	}
	protected void ReSetTitleImageSize()
	{
		TitleImage.ScaleToFit = true;

		TitleImage.Width.Set(DefaultImageSize.X, 0f);
		TitleImage.Height.Set(DefaultImageSize.Y, 0f);

		TitleImage.Left.Set(TitleSelection.Left.Pixels, 0f);
		TitleImage.Top.Set(TitleSelection.Top.Pixels + TitleSelection.Height.Pixels + (separation * 2), 0f);
	}
}
