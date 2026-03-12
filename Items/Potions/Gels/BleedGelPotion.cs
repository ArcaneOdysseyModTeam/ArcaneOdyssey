using ArcaneOdyssey.Buffs.Gels;
using ArcaneOdyssey.Items.Base;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Potions.Gels
{
	public class BleedGelPotion : BaseGelPotion
	{
		public override int GelID => ModContent.BuffType<BleedGel>();

		public override Color LiquidColour => Color.Red;

		public override void AddRecipes()
		{
			CreateRecipe().
				AddIngredient(ItemID.BottledWater).
				AddIngredient(ItemID.Cactus, 20).
				AddTile(TileID.ImbuingStation).
				Register();
		}
	}
}
