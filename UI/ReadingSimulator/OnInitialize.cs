using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ArcaneOdyssey.UI.ReadingSimulator;

// Spoky (2026 Apr 08): If this isn't deleted after the UI is done, then I forgot to delete this
public partial class ReadingSimulatorUI : UIState
{
	#region UI Panel declarations and some stuff related to it
	/// <summary>
	/// Main <see cref="UIPanel"/>, here every other panel will be placed on top of this one
	/// </summary>
	protected UIPanel main = new();

	#region Control Buttons
	/// <summary>
	/// Used for closing this <see cref="ReadingSimulatorUI"/>
	/// </summary>
	protected UIImageButton CloseButton = new(ButtonTextures.Close);
	/// <summary>
	/// Probably unnecessary but I want to add it anyways
	/// </summary>
	protected DragButtonHelper DragButton;

	#region Textures
	public class ButtonTextures
	{
		public static readonly Asset<Texture2D> Close = ArcaneOdysseyMod.Instance.Assets.Request<Texture2D>("UI/SharedImages/CloseButton", AssetRequestMode.ImmediateLoad);

		public class Drag
		{
			public static readonly Asset<Texture2D> Evil = ArcaneOdysseyMod.Instance.Assets.Request<Texture2D>("UI/SharedImages/DragButton/Evil", AssetRequestMode.ImmediateLoad);
			public static readonly Asset<Texture2D> Neutral = ArcaneOdysseyMod.Instance.Assets.Request<Texture2D>("UI/SharedImages/DragButton/Neutral", AssetRequestMode.ImmediateLoad);
			public static readonly Asset<Texture2D> Good = ArcaneOdysseyMod.Instance.Assets.Request<Texture2D>("UI/SharedImages/DragButton/Good", AssetRequestMode.ImmediateLoad);
		}

		public class Page
		{
			public static readonly Asset<Texture2D> Evil = ArcaneOdysseyMod.Instance.Assets.Request<Texture2D>("UI/SharedImages/Generic Button/Evil", AssetRequestMode.ImmediateLoad);
			public static readonly Asset<Texture2D> Neutral = ArcaneOdysseyMod.Instance.Assets.Request<Texture2D>("UI/SharedImages/Generic Button/Neutral", AssetRequestMode.ImmediateLoad);
			public static readonly Asset<Texture2D> Good = ArcaneOdysseyMod.Instance.Assets.Request<Texture2D>("UI/SharedImages/Generic Button/Good", AssetRequestMode.ImmediateLoad);
		}
	}
	#endregion
	#endregion

	public List<ImageButtonButWithAFewExtraThingsForVerySpecificPurposesInTheGuideBookThatShouldNotBeUsedForAnythingElseJeezThisIsALongNameWonderHowMuchCanIPadTheLengthOfThisClass> PageButtons = [];
	public UIList Pages = [];
	public UIScrollbar PageScroller = new();

	public UIPanel BigPanelLmao = new();
	public UIText BigTextLmao = new("", 1.25f);

	#endregion

	#region Initialize (real)
	protected const float separation = 4f;
	public override void OnInitialize()
	{
		#region Main Panel, the panel that serves the main function
		main.SetPadding(0);
		main.BackgroundColor = new(50, 70, 130);

		main.Width.Set(1024f, 0f);
		main.Height.Set(640f, 0f);

		main.HAlign = 0.5f; main.VAlign = 0.4f;

		Append(main);
		#endregion

		#region Shrimple Width, Left, Height and Top Setting for the control buttons
		// Spoky (2026 Apr 30): Reason Drag Button needs a specific class for itself, is because when dragging the UI, the sound effect of mouseOver can sound multiple times
		DragButton = new(this, ButtonTextures.Drag.Neutral);

		int counting = 0;
		float size = 32f;

		UIElement[] buttons = [CloseButton, DragButton];
		foreach (var button in buttons)
		{
			button.Width.Set(size, 0f);
			button.Height.Set(size, 0f);

			button.Top.Set(separation, 0f);
			button.Left.Set(separation + ((size + separation) * counting), 0f);

			counting++;
			main.Append(button);
		}

		#endregion

		CloseButton.OnLeftClick += CloseButton_OnLeftClick;

		#region Debug Thingy
		//if (Player is null || TheBook is null) Main.NewText($"Player is null"); 
		//else
		//{
		//	Main.NewText($"Hmm {Player.name}\n");
		//	foreach (var l in TheBook)
		//	{
		//		Main.NewText($"Name: {l.DisplayName} \n" +
		//			$"\t{l.Description}\n");
		//	}
		//}
		#endregion

		#region Getting the Player
		try
		{
			Player = Main.LocalPlayer;
			CONSUMETHEPAPER();
		}
		catch (Exception ex)
		{
			Main.NewText($"Error getting Player at {nameof(RebootPages)}; error:\n{ex}", new Color(255, 0, 255));
			CommitSudoku();
		}
		#endregion

		// Spoky (2026 Apr 30): Change amounts of buttons here
		#region Option Buttons Handling
		float top = separation + CloseButton.Height.Pixels + (separation * 2),
			height = main.Height.Pixels - (top + separation);

		PageScroller.Width.Set(20f, 0f);
		// Spoky (2026 Apr 30): I have no idea why I have to do this, maybe UIScrollbar has a passive +~8 pixels of height?
		PageScroller.Height.Set(height - 8f, 0f);

		PageScroller.Left.Set(separation, 0f);
		PageScroller.Top.Set(top, 0f);

		Pages.Width.Set(164f, 0f);
		Pages.Height.Set(height, 0f);

		Pages.Left.Set(separation + PageScroller.Left.Pixels + PageScroller.Width.Pixels + (separation * 2), 0f);
		Pages.Top.Set(top, 0f);

		for (int i = 0; i < TheBook.Count; i++)
		{
			ImageButtonButWithAFewExtraThingsForVerySpecificPurposesInTheGuideBookThatShouldNotBeUsedForAnythingElseJeezThisIsALongNameWonderHowMuchCanIPadTheLengthOfThisClass
				button = new(this, i, ButtonTextures.Page.Neutral);
			button.NewPage(TheBook[i]);
			Pages.Add(button);

			//main.Append(button);
			//PageButtons.Add(button);
		}

		main.Append(Pages);
		main.Append(PageScroller);

		Pages.SetScrollbar(PageScroller);

		RebootPages();
		#endregion

		#region Big Text Lmao
		BigPanelLmao.SetPadding(0);
		BigPanelLmao.BackgroundColor = new(73, 94, 171);

		BigPanelLmao.Width.Set(main.Width.Pixels - (Pages.Left.Pixels + Pages.Width.Pixels + (separation * 2) + separation), 0f);
		BigPanelLmao.Height.Set(height, 0f);

		BigPanelLmao.Left.Set(Pages.Left.Pixels + Pages.Width.Pixels + separation, 0f);
		BigPanelLmao.Top.Set(top, 0f);

		main.Append(BigPanelLmao);

		BigTextLmao.Width.Set(BigPanelLmao.Width.Pixels - (separation * 4), 0f);
		BigTextLmao.Height.Set(BigPanelLmao.Width.Pixels - (separation * 4), 0f);

		BigTextLmao.Left.Set(BigPanelLmao.Left.Pixels + separation * 2, 0f);
		BigTextLmao.Top.Set(BigPanelLmao.Top.Pixels + separation * 2, 0f);

		BigTextLmao.IsWrapped = true;

		main.Append(BigTextLmao);
		#endregion
	}

	#endregion
}
