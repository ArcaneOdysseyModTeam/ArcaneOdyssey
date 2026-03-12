using ArcaneOdyssey.Items.Consumable;
using ArcaneOdyssey.UI._BaseImbueUI;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace ArcaneOdyssey.UI.ImbueAcquiring;

// Spoky (2026 Feb 08): If this isn't deleted after the UI is done, then I forgot to delete this
public partial class ImbueAcquireUI : BaseImbueUI
{
	protected override void ChosenButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
	{
		if (ProductSpotLight.CurrentType is not MagicTypes.None)
		{
			Player player = Main.LocalPlayer;
			int acrIndex = player.FindItem(ModContent.ItemType<EagleLegacy>());

			if (acrIndex >= 0)
			{
				player.inventory[acrIndex].TurnToAir();
				if (player.GetItem(player.whoAmI, MagicTypeToItem(ProductSpotLight.CurrentType), GetItemSettings.InventoryEntityToPlayerInventorySettings) is Item newItem && newItem.netID != ItemID.None)
					player.QuickSpawnItem(player.GetSource_FromThis(), newItem, newItem.stack);
				SoundEngine.PlaySound(SoundID.Unlock, player.position);
				YoungMan_KillYourself();
			}
			else
			{
				SoundEngine.PlaySound(SoundID.Tink, player.position);
				Main.NewText($"Did you drop your [i:{ModContent.ItemType<EagleLegacy>()}]legacy!? Pick it up before choosing an option");
			}
		}
		else
		{
			SoundEngine.PlaySound(SoundID.Tink, Main.LocalPlayer.position);
			Main.NewText($"Choose an option first");
		}
	}
}
