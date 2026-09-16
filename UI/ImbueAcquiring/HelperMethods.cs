using ArcaneOdyssey.UI._BaseImbueUI;

namespace ArcaneOdyssey.UI.ImbueAcquiring;

public partial class ImbueAcquireUI : BaseImbueUI
{
	protected override void YoungMan_KillYourself() => ModContent.GetInstance<ModUISystem>().HideTheImbueAcquire();
}
