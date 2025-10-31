using ArcaneOdyssey.Content.Items.Base;
using Terraria;
using Terraria.ID;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Materials
{
    public class Paper : AOBaseItem
    {
        public int AOValue = 1;
        public override AORarities AORarity => AORarities.Common;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = 30;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.value = GalleonToCopper(AOValue);
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.Gel).AddRecipeGroup(RecipeGroupID.Wood).AddTile(TileID.Sawmill).Register();
        }
    }
}
