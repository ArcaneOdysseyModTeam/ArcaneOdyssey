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
	public override void Update(GameTime gameTime)
	{
		if (Main.gameMenu || Main.dedServ) YoungMan_KillYourself();

		// Spoky (2026 Jan 28): Made an oopise, thought I could just set Main.LocalPlayer.mouseInterface to = main.IsMouseHovering, but that breaks every other UI 
		if (main.IsMouseHovering) Main.LocalPlayer.mouseInterface = true;

	}
}
