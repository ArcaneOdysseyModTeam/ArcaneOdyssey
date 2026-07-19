using ArcaneOdyssey.UI._BaseImbueUI;

namespace ArcaneOdyssey.UI.ImbueChange;

public partial class ImbueChangeUI : BaseImbueUI
{
	protected override void YoungMan_KillYourself() => ModContent.GetInstance<ModUISystem>().HideTheImbueChange();
}
