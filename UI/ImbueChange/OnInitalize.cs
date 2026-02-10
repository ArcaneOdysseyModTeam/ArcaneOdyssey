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
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.UI.ImbueChange;

/// <summary>
/// The UI that opens when the player uses a <see cref="Acrimony"/> or a <see cref="StarterAcrimony"/>; this to 
/// </summary>
public partial class ImbueChangeUI : BaseImbueUI
{
	protected override List<MagicTypes> WhoAreWeDoing => GetEnumValues([MagicTypes.None]);

	protected override string GetTitle() => Language.GetTextValue($"{LocalizationPath}SwappingImbue.AnnouncingHeWhoFellOff", TheGuyThatFellOff.Item.Name);

	public ModItem TheGuyThatFellOff;
}
