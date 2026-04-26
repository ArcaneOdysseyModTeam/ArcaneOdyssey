using ArcaneOdyssey.UI._BaseImbueUI;
using Terraria.ModLoader;

namespace ArcaneOdyssey.UI.ImbueAcquiringSequel;

// Spoky (2026 Feb 08): If this isn't deleted after the UI is done, then I forgot to delete this
public partial class ImbueAcquireSequelUI : BaseImbueUI
{
	protected override void YoungMan_KillYourself() => ModContent.GetInstance<ModUISystem>().HideTheImbueSequelAcquire();
}
