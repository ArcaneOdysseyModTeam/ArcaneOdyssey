using System;
using Terraria.UI;

namespace ArcaneOdyssey.UI.SubWorld;

public partial class SubWorldUI : UIState
{

	/// <inheritdoc/>
	public override void Update(GameTime gameTime)
	{
		if (Main.gameMenu || Main.playerInventory)
		{
			CommitSudoku();
			return;
		}

		try
		{
			#region Player Handling
			if (Player is null)
			{
				Player = Main.LocalPlayer;
			}
			if (Player is null)
			{
				Main.NewText($"Player is null, this is bad", new Color(255, 0, 255));
				return;
			}

			#endregion

			#region Quality of Life thingies
			if (main.IsMouseHovering) Player.mouseInterface = true;
			#endregion

			#region Handling Unlock Hint
			bool found = false;
			foreach (UIElement e in SubworldOptions)
			{
				if (e is SubworldPicker picker)
				{
					if (picker.Subworld.MetConditions || !picker.IsMouseHovering) continue;

					if (!UnlockHint.Text.Equals(picker.Subworld.UnlockCondition)) UnlockHint.SetText($"{picker.Subworld.UnlockCondition}");
					UnlockHint.Left.Set(Main.mouseX - 32, 0);
					UnlockHint.Top.Set(Main.mouseY + 32, 0);
					found = true;
				}
				else Main.NewText($"e is not subworldPicker, is {e.GetType()}");
			}
			if (!found) UnlockHint.SetText($"");
			#endregion
		}
		catch (Exception ex)
		{
			Main.NewText($"Error in {nameof(SubWorldUI)}. Error: {ex}", new Color(255, 0, 255));
		}

	}
}
