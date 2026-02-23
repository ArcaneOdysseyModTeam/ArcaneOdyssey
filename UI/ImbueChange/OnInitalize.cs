using ArcaneOdyssey.Content.Items;
using ArcaneOdyssey.Content.Items.Imbues.Relics;
using ArcaneOdyssey.UI._BaseImbueUI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;



namespace ArcaneOdyssey.UI.ImbueChange;

/// <summary>
/// The UI that opens when the player uses a <see cref="Acrimony"/>
/// </summary>
public partial class ImbueChangeUI : BaseImbueUI
{
	protected override List<MagicTypes> WhoAreWeDoing
	{
		get
		{
			if (Main.gameMenu) return [];

			Player player = Main.LocalPlayer;

			if (player is null || !player.active)
			{
				Main.NewText($"Player is null? ? ?", new Color(255, 0, 255));
				return [];
			}

			int itemID = TheGuyThatFellOff.Item.type;

			if (TheGuyThatFellOff is null || itemID <= ItemID.None)
			{
				Main.NewText($"Selected item is null? ? ?", new Color(255, 0, 255));
				return [];
			}

			int eagle = ModContent.ItemType<EaglePatrimony>();

			// Spoky (2026 February 20): I keep forgetting the values inside GetEnumValues are the exceptions
			if (itemID == eagle) return AOUtils.GetEnumValues([MagicTypes.None, MagicTypes.HeHasAcceptedChristInHisHeart]);
			else if (player.HasItem(eagle)) return AOUtils.GetEnumValues([MagicTypes.None, MagicTypes.MonkLife]);
			else return AOUtils.GetEnumValues([MagicTypes.None, MagicTypes.HeHasAcceptedChristInHisHeart]);
		}
	}

	protected override string GetTitle() => Language.GetTextValue($"{LocalizationPath}SwappingImbue.AnnouncingHeWhoFellOff", TheGuyThatFellOff.Item.Name);

	public ModItem TheGuyThatFellOff;
}
