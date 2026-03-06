using ArcaneOdyssey.UI._BaseImbueUI;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;

namespace ArcaneOdyssey.UI.MutateThyMagic;

public partial class MutateThyMagicUI : BaseImbueUI
{
	protected override void YoungMan_KillYourself() => ModContent.GetInstance<ImbueAnythingUISystem>().HideTheMutation();
}
