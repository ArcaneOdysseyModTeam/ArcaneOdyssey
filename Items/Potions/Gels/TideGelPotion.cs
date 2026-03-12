using ArcaneOdyssey.Buffs.Gels;
using ArcaneOdyssey.Items.Base;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Potions.Gels
{
	public class TideGelPotion : BaseGelPotion
	{
		public override int GelID => ModContent.BuffType<TideGel>();

		public override Color LiquidColour => Color.Blue;

		public override void AddRecipes()
		{
			CreateRecipe().
				AddIngredient(ItemID.BottledWater, 5).
				AddTile(TileID.ImbuingStation).
				Register();
		}
	}
}
