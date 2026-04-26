using ArcaneOdyssey.UI._BaseImbueUI;
using Terraria.ModLoader;

namespace ArcaneOdyssey.UI.MutateThyMagic;

public partial class MutateThyMagicUI : BaseImbueUI
{
	protected override void YoungMan_KillYourself() => ModContent.GetInstance<ModUISystem>().HideTheMutation();
}
