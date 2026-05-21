using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;

namespace ArcaneOdyssey.UI.ReadingSimulator;

// Spoky (2026 Apr 08): If this isn't deleted after the UI is done, then I forgot to delete this
public partial class ReadingSimulatorUI : UIState
{
	/// <summary>
	/// Small counter to make visual updates not run every frame, for performance and it is usually unnoticeable 
	/// </summary>
	protected int VisualCounter = 0;
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
				CONSUMETHEPAPER();
			}
			if (Player is null)
			{
				Main.NewText($"Player is null, this is bad", new Color(255, 0, 255));
				CommitSudoku();
				return;
			}

			#endregion

			#region QoL thingies
			if (main.IsMouseHovering) Player.mouseInterface = true;

			if (Dragging)
			{
				main.HAlign = main.VAlign = 0f;

				main.Left.Set(Main.mouseX - (DragButton.Left.Pixels + (DragButton.Width.Pixels / 2)), 0);
				main.Top.Set(Main.mouseY - (DragButton.Top.Pixels + (DragButton.Height.Pixels / 2)), 0);

				Recalculate();
			}
			#endregion

			#region Scroll Handling
			if (PageScroller.IsMouseHovering)
			{
				PlayerInput.LockVanillaMouseScroll("");
			}
			#endregion

			#region Visual thingies
			VisualCounter++;
			if (VisualCounter >= 3)
			{
				VisualCounter = 0;
				VisualUpdate();
			}

			void VisualUpdate()
			{
				DragButton.SetImage(Dragging ? ButtonTextures.Drag.Good : ButtonTextures.Drag.Neutral);
			}
			#endregion
		}
		catch (Exception ex)
		{
			Main.NewText($"Unhandled exception (this is bad); exception:\n{ex}", new Color(255, 0, 255));
			CommitSudoku();
		}
	}


}
