using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Guidebook;
using System;
using System.Collections.Generic;
using Terraria.UI;

namespace ArcaneOdyssey.UI.ReadingSimulator;

public partial class ReadingSimulatorUI : UIState
{
	/// <summary>
	/// Owner of the UI
	/// </summary>
	protected Player Player;
	protected AOPlayer ModPlayer;
	/// <summary>
	/// The Book, he contains the pages.
	/// </summary>
	protected List<GuidebookPage> TheBook;

	protected void CONSUMETHEPAPER()
	{
		if (Player is null) return;
		try { ModPlayer = Player.GetModPlayer<AOPlayer>(); }
		catch (Exception ex)
		{
			Main.NewText($"Error getting {nameof(ModPlayer)} at {nameof(CONSUMETHEPAPER)}; error:\n{ex}", new Color(255, 0, 255));
			CommitSudoku();
		}
		try { TheBook = ModPlayer.AvailablePages(); }
		catch (Exception ex)
		{
			Main.NewText($"Error getting {nameof(TheBook)} at {nameof(CONSUMETHEPAPER)}; error:\n{ex}", new Color(255, 0, 255));
			CommitSudoku();
		}

		#region Debug Thingy
		//Main.NewText($"Hmm {Player.name}\n");
		//foreach (var l in TheBook)
		//{
		//	Main.NewText($"Name: {l.DisplayName} \n" +
		//		$"\t{l.Description}\n");
		//}
		#endregion
	}
}
