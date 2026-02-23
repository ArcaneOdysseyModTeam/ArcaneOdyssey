using ArcaneOdyssey.UI._BaseImbueUI;
using System.Collections.Generic;
using Terraria.Localization;



namespace ArcaneOdyssey.UI.ImbueAcquiringSequel;

// Spoky (2026 Feb 08): If this isn't deleted after the UI is done, then I forgot to delete this
public partial class ImbueAcquireSequelUI : BaseImbueUI
{
	protected override List<MagicTypes> WhoAreWeDoing => AOUtils.GetEnumValues([MagicTypes.None, MagicTypes.MonkLife]);

	protected override string GetTitle() => Language.GetTextValue($"{LocalizationPath}Titles.IfOneIsSoGoodWhyNotTwo");
}
