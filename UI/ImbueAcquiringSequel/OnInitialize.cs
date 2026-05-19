using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.FightingStyles.Normal;
using ArcaneOdyssey.UI._BaseImbueUI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;



namespace ArcaneOdyssey.UI.ImbueAcquiringSequel;

// Spoky (2026 Feb 08): If this isn't deleted after the UI is done, then I forgot to delete this
public partial class ImbueAcquireSequelUI : BaseImbueUI
{
	protected override List<MagicTypes> WhoAreWeDoing
	{
		get
		{
			List<MagicTypes> exceptions = [MagicTypes.None, MagicTypes.MonkLife];

			List<MagicTypes> list = AOUtils.GetEnumValues(exceptions);
			// Spoky (2026 Apr 28): Failsafe, in case this ui somehow opens when a world is not open yet
			if (Main.gameMenu) return list;
			var player = Main.LocalPlayer;

			foreach (var m in list)
			{
				int? id = MagicTypeToID(m);
				if (id is null)
				{
					Main.NewText($"Trying to get id for {m} had an error", new Color(255, 0, 255));
					continue;
				}

				Item item = ContentSamples.ItemsByType[(int)id];
				if (item.ModItem is Imbuable imbue &&
					// Spoky (2026 Apr 28): Basic Combat is exempt of this rule, as a player could want to get a second basic combat to transform one into another while using basic combat as primary
					imbue is not BasicCombat &&
					imbue.PlayerHasImbue(player))
					exceptions.Add(m);
			}

			return AOUtils.GetEnumValues(exceptions);
		}
	}

	protected override string GetTitle() => Language.GetTextValue($"{LocalizationPath}Titles.IfOneIsSoGoodWhyNotTwo");
}
