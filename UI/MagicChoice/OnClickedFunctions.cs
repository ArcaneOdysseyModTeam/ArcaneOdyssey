using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace ArcaneOdyssey.UI.MagicChoice;

public partial class MagicChoiceUIState : UIState
{
	private void CloseButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) => YoungMan_KillYourself();
	private void ChosenButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
	{
		if (ProductSpotLight.CurrentType is not MagicTypes.None)
		{
			Player player = Main.LocalPlayer;

			int index = player.FindItem(ModContent.ItemType<Acrimony>());

			if (index >= 0)
			{
				if (player.GetItem(player.whoAmI, MagicTypeToItem(ProductSpotLight.CurrentType), GetItemSettings.InventoryEntityToPlayerInventorySettings) is Item newItem && newItem.netID != ItemID.None)
					player.QuickSpawnItem(player.GetSource_FromThis(), newItem, newItem.stack);
				player.inventory[index].TurnToAir();
				YoungMan_KillYourself();
			}
			else
			{
				Main.NewText($"Did you drop the acrimony? Pick it up before choosing an option");
			}
		}
		else
		{
			Main.NewText($"Choose an option first");
		}
	}
	private void OptionSelected(UIMouseEvent evt, UIElement listeningElement)
	{
		bool changed = false;
		foreach (var p in TheShop) if (p.BackGround.IsMouseHovering)
		{
			ProductSpotLight.ChangeType(p.CurrentType);
			var item = MagicTypeToItem(p.CurrentType);
			SpotTitle.SetText(item.Name, 1, true);
			if (item.ModItem is AOMagic magic)
			{
				SpotStats.SetText($"Size: {magic.AOScrollSize} \n" +
					$"Speed: {magic.AOScrollSize} \n" +
					$"Damage: {magic.AOScrollDamage} ");
			}
			else if (item.ModItem is FightingStyle fight)
			{
				SpotStats.SetText($"Size: {fight.AOScrollSize} \n" +
					$"Speed: {fight.AOScrollSize} \n" +
					$"Damage: {fight.AOScrollDamage} ");
			}
			else if (item.ModItem is RelicImbue relic)
			{
				SpotStats.SetText($"Size: {relic.AOScrollSize} \n" +
					$"Speed: {relic.AOScrollSize} \n" +
					$"Damage: {relic.AOScrollDamage} ");
			}
			else
			{
				SpotStats.SetText($"Error with {item.Name}");
			}

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
