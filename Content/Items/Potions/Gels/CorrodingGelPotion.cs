using ArcaneOdyssey.Content.Buffs.Gels;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Potions.Gels
{
	public class CorrodingGelPotion : BaseGelPotion
	{
		public override int GelID => ModContent.BuffType<CorrodingGel>();

		public override Color LiquidColour => Color.Purple;

		public override void AddRecipes()
		{
			CreateRecipe().
				AddIngredient(ItemID.BottledWater).
				AddIngredient(ItemID.JungleSpores, 6).
				AddTile(TileID.ImbuingStation).
				Register();
		}
	}
}
