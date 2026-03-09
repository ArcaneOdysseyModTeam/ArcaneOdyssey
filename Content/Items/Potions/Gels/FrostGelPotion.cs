using ArcaneOdyssey.Content.Buffs.Gels;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Potions.Gels
{
	public class FrostGelPotion : BaseGelPotion
	{
		public override int GelID => ModContent.BuffType<ArcticGel>();

		public override Color LiquidColour => Color.LightBlue;

		public override void AddRecipes()
		{
			CreateRecipe().
				AddIngredient(ItemID.BottledWater).
				AddIngredient(ItemID.IceBlock, 10).
				AddTile(TileID.ImbuingStation).
				Register();
		}
	}
}
