using System;
using Terraria.Chat;
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
		}
		catch (Exception ex)
		{
			Main.NewText($"Error in {nameof(SubWorldUI)}. Error: {ex}", new Color(255, 0, 255));
		}

	}
}
