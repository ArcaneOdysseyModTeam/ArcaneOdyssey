using ArcaneOdyssey.UI._BaseImbueUI;
using System.Collections.Generic;
using Terraria.Localization;



namespace ArcaneOdyssey.UI.ImbueAcquiring;

public partial class ImbueAcquireUI : BaseImbueUI
{
	protected override List<MagicTypes> WhoAreWeDoing => AOUtils.GetEnumValues([MagicTypes.None, MagicTypes.HeHasAcceptedChristInHisHeart]);

	protected override string GetTitle() => Language.GetTextValue($"{LocalizationPath}Titles.ImbueAcquire");
}
