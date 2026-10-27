using ArcaneOdyssey.Subworlds.Base;
using System;
using Terraria.Audio;
using Terraria.UI;

namespace ArcaneOdyssey.UI.SubWorld;

public partial class SubWorldUI : UIState
{
	private void EnterButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
	{
		if (ChosenSubworld is null) return;
	}

	#region Close button and Close method
	private void CloseButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) => CommitSudoku();

	/// <summary>
	/// Funny, when the method name is funny, funny
	/// </summary>
	protected void CommitSudoku()
	{
		SoundEngine.PlaySound(SoundID.MenuClose, Main.LocalPlayer.position);
		ModContent.GetInstance<ModUISystem>().HideSubWorld();
	}
	#endregion

	#region AT EASE WITH THE HARMONY
	public void AtEaseWithTheHarmonyIAm(AOSubworld harmony, LocalizedText title, Asset<Texture2D> image, string description) => AtEaseWithTheHarmonyIAm(harmony, title.Value, image, description);
	public void AtEaseWithTheHarmonyIAm(AOSubworld harmony, string title, Asset<Texture2D> image, string description)
	{
		if (harmony == null) return;
		if (harmony == ChosenSubworld || !harmony.MetConditions) ChosenSubworld = null;
		else ChosenSubworld = harmony;

		if (ChosenSubworld == null)
		{
			SoundEngine.PlaySound(SoundID.MenuClose, Player.position);

			TitleSelection.SetText($"");
			TitleImage.Color = new Color(0, 0, 0, 0);
			TitleDescription.SetText($"");

			EnterText.SetText("");
			EnterButton.IgnoresMouseInteraction = true;
			EnterButton.BackgroundColor = new(50, 70, 130);
		}
		else
		{
			SoundEngine.PlaySound(SoundID.MenuOpen, Player.position);

			TitleSelection.SetText(title);

			TitleImage.Color = Color.White;
			TitleImage.SetImage(image);
			ReSetTitleImageSize();

			TitleDescription.SetText(description);

			EnterText.SetText(Language.GetText("Mods.ArcaneOdyssey.UI.Subworld.Enter"));
			EnterButton.IgnoresMouseInteraction = false;
			EnterButton.BackgroundColor = new(73, 94, 171);
		}
	}
	#endregion
}
