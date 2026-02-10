using ArcaneOdyssey.UI._BaseImbueUI;
using Terraria.ModLoader;

namespace ArcaneOdyssey.UI.ImbueAcquiring;

public partial class ImbueAcquireUI : BaseImbueUI
{
	protected override void YoungMan_KillYourself() => ModContent.GetInstance<ImbueAnythingUISystem>().HideTheImbueAcquire();
}
