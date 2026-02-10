using ArcaneOdyssey.Content.Buffs.Gels;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Potions.Gels
{
	public class ArcticGelPotion : BaseGelPotion
	{
		public override int GelID => ModContent.BuffType<ArcticGel>();

		public override Color LiquidColour => Color.White;
	}
}
