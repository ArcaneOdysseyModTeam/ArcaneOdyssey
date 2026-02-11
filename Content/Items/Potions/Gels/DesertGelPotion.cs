using ArcaneOdyssey.Content.Buffs.Gels;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Potions.Gels
{
	public class DesertGelPotion : BaseGelPotion
	{
		public override int GelID => ModContent.BuffType<DesertGel>();

		public override Color LiquidColour => Color.SandyBrown;

		public override void AddRecipes()
		{
			CreateRecipe().
				AddIngredient(ItemID.BottledWater).
				AddIngredient(ItemID.SandBlock, 10).
				AddTile(TileID.ImbuingStation).
				Register();
		}
	}
}
