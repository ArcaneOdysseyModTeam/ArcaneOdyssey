using ArcaneOdyssey.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Base
{
	public abstract class MagicScroll : AnyScroll, ILocalizedModType
	{
		public override string LocalizationCategory => "Magic.Scrolls";
	}
}
