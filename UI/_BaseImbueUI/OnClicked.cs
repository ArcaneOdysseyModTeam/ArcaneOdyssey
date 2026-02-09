using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Normal;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using ArcaneOdyssey.Content.Items.Imbues.Relics;
using ArcaneOdyssey.Content.Items.Materials;
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
using Terraria.Graphics;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace ArcaneOdyssey.UI._BaseImbueUI;

public abstract partial class BaseImbueUI : UIState
{
	protected virtual void CloseButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(SoundID.MenuClose, Main.LocalPlayer.position);
		YoungMan_KillYourself();
	}
	protected abstract void ChosenButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement);
	protected virtual void OptionSelected(UIMouseEvent evt, UIElement listeningElement)
	{
		bool changed = false;
		foreach (var p in TheShop) if (p.BackGround.IsMouseHovering || p.Icon.IsMouseHovering)
		{
			SoundEngine.PlaySound(SoundID.MenuOpen, Main.LocalPlayer.position);

			ProductSpotLight.ChangeType(p.CurrentType);
			var item = MagicTypeToItem(p.CurrentType).Clone();

			SpotTitle.SetText(item.Name, 1, true);
			if (item.ModItem is AOMagic magic)
			{
				// Spoky (2026 Feb 05): Doesn't work? Maybe it does?
				string prefix = magic.ImbueDebuffs.Length switch
				{
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
					$"Speed: {magic.AOScrollSize} \n" +
					$"Damage: {magic.AOScrollDamage} \n" +
					$"{prefix} {text}");
			}
			else if (item.ModItem is FightingStyle fight)
			{
				SpotStats.SetText($"Size: {fight.AOScrollSize} \n" +
					$"Speed: {fight.AOScrollSize} \n" +
					$"Damage: {fight.AOScrollDamage} ");
			}
			else if (item.ModItem is SpiritImbue relic)
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
