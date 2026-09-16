using ArcaneOdyssey.Buffs.Gels;
using ArcaneOdyssey.Items.Base;

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
