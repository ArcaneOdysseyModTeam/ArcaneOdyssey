using ArcaneOdyssey.UI._BaseImbueUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace ArcaneOdyssey.UI.MutateThyMagic;

public partial class MutateThyMagicUI : BaseImbueUI
{
	protected override void ChosenButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
	{
		
	}

	protected override void OptionChosen(Product p)
	{
		SoundEngine.PlaySound(SoundID.MenuOpen, Main.LocalPlayer.position);
		var item = MagicTypeToItem(p.CurrentType).Clone();

		bool doingASilly = Main.rand.NextBool(100);

		string suffix;
		if (doingASilly)
		{
			suffix = p.CurrentType switch
			{
				MagicTypes.Acid or MagicTypes.Sand or MagicTypes.Sand or MagicTypes.Shadow => PickOne(["AndJustGoingToTheToilet", "ANDORDERING54NUGGETS"]),

				MagicTypes.Ash or MagicTypes.Crystal or MagicTypes.Magma or MagicTypes.Glass => PickOne(["AndScanning500Coupons", "AndJustBuyingABigMac", "AndJustGoingToTheToilet"]),

				MagicTypes.Earth or MagicTypes.Explosion or MagicTypes.Fire or MagicTypes.Lightning or MagicTypes.Earth => PickOne(["AndOrderingSomethingActuallyInteresting", "ANDORDERING54NUGGETS"]),

				MagicTypes.Light or MagicTypes.Metal or MagicTypes.Plasma or MagicTypes.Poison => PickOne(["AndJustBuyingABigMac", "ANDORDERING54NUGGETS", "AndJustGoingToTheToilet"]),

				MagicTypes.Ice or MagicTypes.Snow or MagicTypes.Water or MagicTypes.Wind or MagicTypes.Wood => PickOne(["AndOrderingSodaWITHEXTRAICE"]),

				MagicTypes.None or MagicTypes.ReturnToMonke or MagicTypes.MonkLife or MagicTypes.HeHasAcceptedChristInHisHeart or _ => "AndWaitWaitWhat",
			};
		}
		else suffix = "AndEatingLikeANormalPerson";

		TitleText.SetText(Language.GetTextValue($"{LocalizationPath}Titles.{suffix}", item.Name));

		static string PickOne(List<string> strings) => strings[Main.rand.Next(strings.Count)];
	}
}
