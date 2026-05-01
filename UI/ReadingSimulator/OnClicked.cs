using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace ArcaneOdyssey.UI.ReadingSimulator;

// Spoky (2026 Apr 08): If this isn't deleted after the UI is done, then I forgot to delete this
public partial class ReadingSimulatorUI : UIState
{
	#region Closing and Closing Button clicked
	protected void CloseButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) => CommitSudoku();
	protected void CommitSudoku()
	{
		SoundEngine.PlaySound(SoundID.MenuClose, Main.LocalPlayer.position);
		ModContent.GetInstance<ModUISystem>().HideReadingSimulator();
	}
	#endregion

	#region Page Selection and Page Refreshing
	/// <summary>
	/// The index of the <see cref="PageButtons"/> chosen; if it is -1, then no page has been selected
	/// </summary>
	protected int ChosenPage = -1;
	public void RebootPages()
	{
		if (PageButtons.Count <= 0)
		{
			return;
		}

		if (TheBook is null || TheBook.Count <= 0)
		{
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
		}

		for (int i = 0; i < PageButtons.Count; i++)
		{
			if (i >= TheBook.Count)
			{
				PageButtons[i].SetImage(ButtonTextures.Page.Evil);
			}
			else
			{
				PageButtons[i].SetImage(ChosenPage == i ? ButtonTextures.Page.Good : ButtonTextures.Page.Neutral);
				PageButtons[i].NewPage(TheBook[i]);
			}
		}
	}
	#endregion

	#region Draggable Capability, sponsored by example mod's draggable ui panel
	private Vector2 offset;
	/// <summary>
	/// A flag that checks if the panel is currently being dragged
	/// </summary>
	private bool Dragging;

	protected void DragStart(UIMouseEvent evt)
	{
		offset = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);
		Dragging = true;


		Recalculate();
		SoundEngine.PlaySound(SoundID.MenuOpen, Player.position);
	}
	protected void DragEnd(UIMouseEvent evt)
	{
		Dragging = false;

		Recalculate();
		SoundEngine.PlaySound(SoundID.MenuClose, Player.position);
	}

	#endregion
}
