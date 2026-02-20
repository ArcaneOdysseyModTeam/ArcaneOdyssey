using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Walls;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Blocks
{
	public class BronzeSlabWallItem : AOBaseItem
	{
		public override AORarities AORarity => AORarities.Common;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DefaultToPlaceableWall(ModContent.WallType<BronzeSlabWall>());
		}

		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient<BronzeSlab>().AddTile(TileID.WorkBenches).Register();
		}
	}
}
