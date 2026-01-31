using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace ArcaneOdyssey.UI.MagicChoice;

public partial class MagicChoiceUIState : UIState
{
	private void CloseButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement) => YoungMan_KillYourself();
	private void OptionSelected(UIMouseEvent evt, UIElement listeningElement)
	{
		bool changed = false;
		foreach (var p in TheShop) if (p.BackGround.IsMouseHovering)
			{
				ProductSpotLight.ChangeType(p.CurrentType);
				changed = true;
			}
		if (!changed && ProductSpotLight.CurrentType is not MagicTypes.None) ProductSpotLight.ChangeType(MagicTypes.None);
	}
}
