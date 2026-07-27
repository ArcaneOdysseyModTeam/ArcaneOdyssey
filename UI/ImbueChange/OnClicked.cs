using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Consumable;
using ArcaneOdyssey.UI._BaseImbueUI;
using Terraria.Audio;
using Terraria.UI;
using static MagicStorage.UI.UISlotZone;

namespace ArcaneOdyssey.UI.ImbueChange;

// Spoky (2026 Feb 09): If this isn't deleted after the UI is done, then I forgot to delete this
public partial class ImbueChangeUI : BaseImbueUI
{
	protected override void OptionChosen(MagicProduct p)
	{
		base.OptionChosen(p);

		//Main.NewText($"Check, felloff: {TheGuyThatFellOff.Item.Name}, spot: {SpotTitle.Text}, hmm: {string.Equals(TheGuyThatFellOff.Item.Name.ToLower(), SpotTitle.Text.ToLower())}");
		string suffix = string.Equals(TheGuyThatFellOff.Item.Name.ToLower(), SpotTitle.Text.ToLower()) ?
			"PleaseDontTellMeThisGuyIsAboutToWasteTheirAcrimony" :
			"BetrayalAmogstUs";
		TitleText.SetText(Language.GetTextValue($"{LocalizationPath}SwappingImbue.{suffix}", TheGuyThatFellOff.Item.Name, SpotTitle.Text));
	}

	protected override void ChosenButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
	{
		if (ProductSpotLight.CurrentType is not MagicTypes.None)
		{
			Player player = Main.LocalPlayer;
			int acrIndex = player.FindItem(ModContent.ItemType<Acrimony>()), imbuIndex = player.FindItem(TheGuyThatFellOff.Type);

			//Main.NewText($"Player still has imbueable {player.HasItem(TheGuyThatFellOff.Type)} [i:{TheGuyThatFellOff.Type}], \n" +
			//	$"Index: {imbuIndex}, Acrindex: {acrIndex}");

			if (acrIndex >= 0 && imbuIndex >= 0)
			{
				var og = player.inventory[imbuIndex].ModItem as MagicType;
				player.inventory[imbuIndex].SetDefaults(MagicTypeToItem(ProductSpotLight.CurrentType).type);
				player.inventory[acrIndex].TurnToAir();


				var newItem = player.inventory[imbuIndex];

				if (newItem.ModItem is MagicType magic)
				{
					magic.Skills = og.Skills;
				}

				SoundEngine.PlaySound(SoundID.Unlock);
				YoungMan_KillYourself();
			}
			else if (acrIndex <= 0)
			{
				SoundEngine.PlaySound(SoundID.Tink);
				Main.NewText($"Did you drop the acrimony? Pick it up before choosing an option");
			}
			else if (imbuIndex <= 0)
			{
				SoundEngine.PlaySound(SoundID.Tink);
				Main.NewText($"Have you already managed to lose your [i:{TheGuyThatFellOff.Type}]{TheGuyThatFellOff.Item.Name}, I'm actually impressed! At your incompetence");
			}
		}
		else
		{
			SoundEngine.PlaySound(SoundID.Tink);
			Main.NewText($"Choose an option first");
		}
	}
}
