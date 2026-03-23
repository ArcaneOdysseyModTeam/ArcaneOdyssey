using ArcaneOdyssey.Imbues.Base;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
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

	protected virtual void OptionChosen(MagicProduct p)
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

			SpotStats.SetText($"Size: {magic.ScrollSize} \n" +
				$"Speed: {magic.ScrollSpeed} \n" +
				$"Damage: {magic.ScrollDamage} \n" +
				$"{prefix} {text}");
		}
		else if (item.ModItem is Imbuable other)
		{
			SpotStats.SetText($"Size: {other.ScrollSize} \n" +
				$"Speed: {other.ScrollSpeed} \n" +
				$"Damage: {other.ScrollDamage} ");
		}
		else
		{
			SpotStats.SetText($"Error with {item.Name}");
		}
	}
	protected virtual void OptionSelected(UIMouseEvent evt, UIElement listeningElement)
	{
		bool changed = false;
		foreach (MagicProduct p in TheShop) if (p.BackGround.IsMouseHovering || p.Icon.IsMouseHovering)
			{
				OptionChosen(p);

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
