using ArcaneOdyssey.UI._BaseImbueUI;

namespace ArcaneOdyssey.UI.MutateThyMagic;

public partial class MutateThyMagicUI : BaseImbueUI
{
	protected override void _VisualUpdateExtras()
	{
		foreach (CustomProduct e in TodaysOffers) e.Update();
	}

	protected override void _UpdateExtras()
	{
		if (AuxPanel.IsMouseHovering) Main.LocalPlayer.mouseInterface = true;
	}
}
