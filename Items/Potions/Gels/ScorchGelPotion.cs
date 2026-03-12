using ArcaneOdyssey.Buffs.Gels;
using ArcaneOdyssey.Items.Base;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Potions.Gels
{
	public class ScorchGelPotion : BaseGelPotion
	{
		public override int GelID => ModContent.BuffType<ScorchGel>();

		public override Color LiquidColour => new(168, 0, 135);

		public override void AddRecipes()
		{
			CreateRecipe().
				AddIngredient(ItemID.BottledWater).
				AddIngredient(ItemID.Amethyst, 10).
				AddTile(TileID.ImbuingStation).
				Register();
		}
	}
}
