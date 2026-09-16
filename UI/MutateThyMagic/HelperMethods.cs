using ArcaneOdyssey.UI._BaseImbueUI;

namespace ArcaneOdyssey.UI.MutateThyMagic;

public partial class MutateThyMagicUI : BaseImbueUI
{
	protected override void YoungMan_KillYourself() => ModContent.GetInstance<ModUISystem>().HideTheMutation();
}
