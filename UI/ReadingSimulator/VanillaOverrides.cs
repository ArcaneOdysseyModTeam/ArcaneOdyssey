using Terraria;
using Terraria.UI;

namespace ArcaneOdyssey.UI.ReadingSimulator;

public partial class ReadingSimulatorUI : UIState
{
	public override void LeftMouseDown(UIMouseEvent evt)
	{
		if (DragButton.IsMouseHovering) DragStart(evt);
	}

	public override void LeftMouseUp(UIMouseEvent evt)
	{
		if (Dragging) DragEnd(evt);
	}
	public override void OnActivate()
	{
		if (Main.gameMenu) return;
		Player = Main.LocalPlayer;
		CONSUMETHEPAPER();
	}
}
