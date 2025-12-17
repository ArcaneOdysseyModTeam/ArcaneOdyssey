using ArcaneOdyssey.Content.Items.Base;
using Terraria;
using Terraria.ID;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Materials
{
	public class EmptyScroll : AOBaseItem
	{
		public override AORarities AORarity => AORarities.Uncommon;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = 32;
			Item.height = 32;
			Item.value = GalleonToCopper(15);
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<Paper>(10).AddTile(TileID.Bookcases).Register();
		}
	}
}
