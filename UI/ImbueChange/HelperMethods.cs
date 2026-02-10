using ArcaneOdyssey.UI._BaseImbueUI;
using Terraria.ModLoader;

namespace ArcaneOdyssey.UI.ImbueChange;

public partial class ImbueChangeUI : BaseImbueUI
{
	protected override void YoungMan_KillYourself() => ModContent.GetInstance<ImbueAnythingUISystem>().HideTheImbueChange();
}
