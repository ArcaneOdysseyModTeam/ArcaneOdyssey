using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;

namespace ArcaneOdyssey.UI._BaseImbueUI;

// Spoky (2026 Feb 08): If this isn't deleted after the UI is done, then I forgot to delete this
public abstract partial class BaseImbueUI : UIState
{
	public override void Update(GameTime gameTime)
	{
		if (Main.gameMenu || Main.dedServ) YoungMan_KillYourself();

		// Spoky (2026 Jan 28): Made an oopise, thought I could just set Main.LocalPlayer.mouseInterface to = main.IsMouseHovering, but that breaks every other UI 
		if (main.IsMouseHovering || CloseButton.IsMouseHovering || ChooseButton.IsMouseHovering) Main.LocalPlayer.mouseInterface = true;

		#region Visual Changes for the Products 
		VisualUpdate(); void VisualUpdate()
		{
			// Spoky (2026 Feb 05): Unsure if keeping this counter so it runs once every 2 ticks instead of every tick; Can't tell if running it every tick might make it too resource intensive

			//VisualUpdateCounter++;
			//if (VisualUpdateCounter < 2) return;
			//VisualUpdateCounter = 0;

			foreach (var p in TheShop) p.Update();
		}
		#endregion
	}
	private int VisualUpdateCounter = 0;
}
