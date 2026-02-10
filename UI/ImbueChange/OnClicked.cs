using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Normal;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using ArcaneOdyssey.Content.Items.Imbues.Relics;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.UI._BaseImbueUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace ArcaneOdyssey.UI.ImbueChange;

// Spoky (2026 Feb 09): If this isn't deleted after the UI is done, then I forgot to delete this
public partial class ImbueChangeUI : BaseImbueUI
{
	protected override void OptionChosen(Product p)
	{
		base.OptionChosen(p);

		TitleText.SetText(Language.GetTextValue($"{LocalizationPath}SwappingImbue.BetrayalAmogstUs", TheGuyThatFellOff.Item.Name, SpotTitle.Text));
	}
	protected override void ChosenButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
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
}
