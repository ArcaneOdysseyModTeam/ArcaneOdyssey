using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.UI._BaseImbueUI;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.ModLoader;

using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.UI.ImbueChange;

/// <summary>
/// The UI that opens when the player uses a <see cref="Acrimony"/> or a <see cref="StarterAcrimony"/>; this to 
/// </summary>
public partial class ImbueChangeUI : BaseImbueUI
{
	protected override List<MagicTypes> WhoAreWeDoing => GetEnumValues([MagicTypes.None, MagicTypes.HeHasAcceptedChristInHisHeart]);

	protected override string GetTitle() => Language.GetTextValue($"{LocalizationPath}SwappingImbue.AnnouncingHeWhoFellOff", TheGuyThatFellOff.Item.Name);

	public ModItem TheGuyThatFellOff;
}
