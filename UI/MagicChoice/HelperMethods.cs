using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace ArcaneOdyssey.UI.MagicChoice;

public partial class MagicChoiceUIState : UIState
{
	/// <summary>
	/// Makes this <see cref="UIState"/> commit sudoku
	/// </summary>
	protected void YoungMan_KillYourself() => ModContent.GetInstance<MagicChoiceUISystem>().HideTheUI();
}
