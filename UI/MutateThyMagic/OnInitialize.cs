using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.UI._BaseImbueUI;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArcaneOdyssey.UI.MutateThyMagic;

public partial class MutateThyMagicUI : BaseImbueUI
{
	#region UI Panels declaration (but not setting the values)
	/// <summary>
	/// The second Shop containing the special offers of the day
	/// </summary>
	protected UIPanel AuxPanel = new();

	/// <summary>
	/// Same as <see cref="BaseImbueUI.TheShop"/>, but for the mutations
	/// </summary>
	protected List<CustomProduct> TodaysOffers = [];

	/// <summary>
	/// The title that displays which mutation is being selected
	/// </summary>
	protected UIText AuxTitle = new("hmm");
	#endregion

	protected override List<MagicTypes> WhoAreWeDoing
	{
		get 
		{
			List<MagicTypes> types = [];
			foreach (Item i in Main.LocalPlayer.inventory)
			{
				//Main.NewText($"hmm {i.Name}: {i.ModItem is Imbuable}, Magic?: {i.ModItem is AOMagic}");
				ModItem item = i.ModItem;
				if (item is AOMagic magic && magic.ImbuableTier is ImbuableTiers.Normal)
				{
					MagicTypes type = IDToMagicType(magic.Type);
					if (type is not MagicTypes.None) types.Add(type);
				}
			}
			return types; 
		}
	}

	protected override string GetTitle() => Language.GetTextValue($"{LocalizationPath}Titles.WeAreGoingToMCDonalds");

	protected override void _OnInitializeExtras()
	{
		// Spoky (2026 March 07): None of these two will be appened here, they will be when they are needed
		#region Aux Title
		AuxTitle.HAlign = 0.5f;
		AuxTitle.VAlign = 0.2f;

		//int mainTop = ((64 + Separation) * TotalRows) + Separation;
		AuxTitle.Top.Set(main.Height.Pixels + Separation, 0f);
		#endregion

		#region Aux Panel
		AuxPanel.HAlign = AuxTitle.HAlign;
		AuxPanel.VAlign = AuxTitle.VAlign;

		AuxPanel.SetPadding(0);
		AuxPanel.BackgroundColor = new(73, 94, 171);

		AuxPanel.Width.Set(main.Width.Pixels, 0f);
		// Spoky (2026 Mars 07): Seems like height for uitext doesn't get set automatically
		AuxPanel.Top.Set(AuxTitle.Top.Pixels + /*AuxTitle.Height.Pixels */ 48 + Separation, 0f);
		#endregion
	}
}
