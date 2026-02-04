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

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Item.ResearchUnlockCount = 25;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = 30;
			Item.height = 32;
			Item.maxStack = Item.CommonMaxStack;
			Item.value = GalleonToCopper(AOValue);
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient(ItemID.Gel).AddRecipeGroup(RecipeGroupID.Wood).AddTile(TileID.Sawmill).Register();
			Recipe.Create(ItemID.PaperAirplaneA, 5).AddIngredient(Type).Register();
			Recipe.Create(ItemID.PaperAirplaneB, 5).AddIngredient(Type).Register();
			Recipe.Create(ItemID.Book).AddIngredient(Type).AddTile(TileID.WorkBenches).Register();
		}
	}
}
