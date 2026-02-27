using ArcaneOdyssey.Content.Buffs.Gels;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Potions.Gels
{
	public class MeltingGelPotion : BaseGelPotion
	{
		public override int GelID => ModContent.BuffType<MeltingGel>();

		public override Color LiquidColour => Color.OrangeRed;

		public override void AddRecipes()
		{
			CreateRecipe().
				AddIngredient(ItemID.Bottle).
				AddIngredient(ItemID.LavaBucket, 3).
				AddTile(TileID.ImbuingStation).
				Register();
		}
	}
}
