using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Walls;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Blocks
{
	public class BronzeBrickWallItem : AOBaseItem
	{
		public override AORarities AORarity => AORarities.Common;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DefaultToPlaceableWall(ModContent.WallType<BronzeBrickWall>());
		}

		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient<BronzeBrick>().AddTile(TileID.WorkBenches).Register();
		}
	}
}
