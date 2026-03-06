using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.UI._BaseImbueUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace ArcaneOdyssey.UI.MutateThyMagic;

public partial class MutateThyMagicUI : BaseImbueUI
{
	protected override List<MagicTypes> WhoAreWeDoing
	{
		get 
		{
			List<MagicTypes> types = [];
			foreach (Item i in Main.LocalPlayer.inventory)
			{
				//Main.NewText($"hmm {i.Name}: {i.ModItem is Imbuable}, Magic?: {i.ModItem is AOMagic}");
				ModItem item = i.ModItem;
				if (item is AOMagic magic && magic.ImbuableTier is AOImbuableTier.Normal)
				{
					MagicTypes type = IDToMagicType(magic.Type);
					if (type is not MagicTypes.None) types.Add(type);
				}
			}
			return types; 
		}
	}

	protected override string GetTitle() => Language.GetTextValue($"{LocalizationPath}Titles.WeAreGoingToMCDonalds");
}
