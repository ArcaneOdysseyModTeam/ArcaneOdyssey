using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace ArcaneOdyssey.UI.MagicChangeOLD;

public partial class MagicChoiceUIState : UIState
{
	private void CloseButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(SoundID.MenuClose, Main.LocalPlayer.position);
		YoungMan_KillYourself();
	}
	private void ChosenButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
	{
		if (ProductSpotLight.CurrentType is not MagicTypes.None)
		{
			Player player = Main.LocalPlayer;
			int acrIndex = player.FindItem(ModContent.ItemType<StarterAcrimony>()), imbuIndex = player.FindItem(TheGuyThatFellOff.Type);
			if (acrIndex < 0) acrIndex = player.FindItem(ModContent.ItemType<Acrimony>());

			//Main.NewText($"Player still has imbueable {player.HasItem(TheGuyThatFellOff.Type)} [i:{TheGuyThatFellOff.Type}], \n" +
			//	$"Index: {imbuIndex}, Acrindex: {acrIndex}");

			if (acrIndex >= 0 && imbuIndex >= 0)
			{
				player.inventory[acrIndex].TurnToAir();
				player.inventory[imbuIndex].TurnToAir();
				if (player.GetItem(player.whoAmI, MagicTypeToItem(ProductSpotLight.CurrentType), GetItemSettings.InventoryEntityToPlayerInventorySettings) is Item newItem && newItem.netID != ItemID.None)
					player.QuickSpawnItem(player.GetSource_FromThis(), newItem, newItem.stack);
				SoundEngine.PlaySound(SoundID.Unlock, player.position);
				YoungMan_KillYourself();
			}
			else if (acrIndex <= 0)
			{
				SoundEngine.PlaySound(SoundID.Tink, player.position);
				Main.NewText($"Did you drop the acrimony? Pick it up before choosing an option");
			}
			else if (imbuIndex <= 0)
			{
				SoundEngine.PlaySound(SoundID.Tink, player.position);
				Main.NewText($"Have you already managed to lose your [i:{TheGuyThatFellOff.Type}]{TheGuyThatFellOff.Item.Name}, I'm actually impressed! At your incompetence");
			}
		}
		else
		{
			SoundEngine.PlaySound(SoundID.Tink, Main.LocalPlayer.position);
			Main.NewText($"Choose an option first");
		}
	}
	private void OptionSelected(UIMouseEvent evt, UIElement listeningElement)
	{
		bool changed = false;
		foreach (var p in TheShop) if (p.BackGround.IsMouseHovering)
		{
			SoundEngine.PlaySound(SoundID.MenuOpen, Main.LocalPlayer.position);

			ProductSpotLight.ChangeType(p.CurrentType);
			var item = MagicTypeToItem(p.CurrentType).Clone();

			SpotTitle.SetText(item.Name, 1, true);
			if (item.ModItem is AOMagic magic)
			{
				// Spoky (2026 Feb 05): Doesn't work? Maybe it does?
				string prefix = magic.ImbueDebuffs.Length switch {
					> 1 => "Status Effects:",
					1 => "Status Effect:",
					_ => "",
				},
				text = "";
				if (magic.ImbueDebuffs.Length > 1)
				{
					for (int i = 0; i < magic.ImbueDebuffs.Length; i++)
					{
						string imbue = Lang.GetBuffName(magic.ImbueDebuffs[i].debuffID);
						text += i < magic.ImbueDebuffs.Length - 1 ? $"{imbue}, " : $"{imbue}";
					}
				}
				else if (magic.ImbueDebuffs.Length == 1) text = $"{Lang.GetBuffName(magic.ImbueDebuffs[0].debuffID)}";

				SpotStats.SetText($"Size: {magic.AOScrollSize} \n" +
					$"Speed: {magic.AOScrollSpeed} \n" +
					$"Damage: {magic.AOScrollDamage} \n" +
					$"{prefix} {text}");
			}
			else if (item.ModItem is FightingStyleBarred fight)
			{
				SpotStats.SetText($"Size: {fight.MinScrollSize} - {fight.MaxScrollSize} \n" +
					$"Speed: {fight.MinScrollSpeed} - {fight.MaxScrollSpeed} \n" +
					$"Damage: {fight.MinScrollDamage} - {fight.MaxScrollDamage} ");
			}
			else if (item.ModItem is Imbuable other)
			{
				SpotStats.SetText($"Size: {other.AOScrollSize} \n" +
					$"Speed: {other.AOScrollSpeed} \n" +
					$"Damage: {other.AOScrollDamage} ");
			}
			else
			{
				SpotStats.SetText($"Error with {item.Name}");
			}

			HeFellOff.SetText(Language.GetTextValue($"{LocalizationPath}BetrayalAmogstUs", TheGuyThatFellOff.Item.Name, SpotTitle.Text));

			changed = true;
			break;
		}
		if (!changed && ProductSpotLight.CurrentType is not MagicTypes.None)
		{
			ProductSpotLight.ChangeType(MagicTypes.None);
			SpotTitle.SetText("");
			SpotStats.SetText("");
		}
	}
}
