using ArcaneOdyssey.Content.Mounts;
using Terraria.ModLoader;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Items.Equipment
{
	public class WoodenPaddle : ModItem
	{
		public AORarities AORarity = AORarities.Common;
		public override void SetDefaults()
		{
			Item.width = Item.height = 60;
			Item.mountType = ModContent.MountType<Rowboat>();
			Item.value = 1000;
		}
        public override void AddRecipes()
        {
			CreateRecipe().AddIngredient(ItemID.PalmWood, 100).AddTile(TileID.WorkBenches).Register();
        }
	}
}
