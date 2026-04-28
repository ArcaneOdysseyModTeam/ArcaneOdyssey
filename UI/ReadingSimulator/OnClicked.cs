using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace ArcaneOdyssey.UI.ReadingSimulator;

// Spoky (2026 Apr 08): If this isn't deleted after the UI is done, then I forgot to delete this
public partial class ReadingSimulatorUI : UIState
{
	protected virtual void CloseButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(SoundID.MenuClose, Main.LocalPlayer.position);
		ModContent.GetInstance<ModUISystem>().HideReadingSimulator();
	}
}
