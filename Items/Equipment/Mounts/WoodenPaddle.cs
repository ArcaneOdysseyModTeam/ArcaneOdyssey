using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Mounts;

namespace ArcaneOdyssey.Items.Equipment.Mounts
{
	public class WoodenPaddle : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Common;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 30;
			Item.mountType = ModContent.MountType<Rowboat>();
			Item.value = 1000;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient(ItemID.PalmWood, 100).AddTile(TileID.WorkBenches).Register();
		}
	}
}
