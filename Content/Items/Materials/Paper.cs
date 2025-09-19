using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Materials
{
    public class Paper : ModItem
    {
        public int AOValue = 1;
        public AORarities AORarity = AORarities.Common;

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.rare = (int)AORarity;
            Item.value = GalleonToCopper(AOValue);
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.Gel).AddRecipeGroup(RecipeGroupID.Wood).AddTile(TileID.Sawmill).Register();
        }
    }
}
