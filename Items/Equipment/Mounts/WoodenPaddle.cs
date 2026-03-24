using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Mounts;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Equipment.Mounts
{
	public class WoodenPaddle : BaseItem
	{
		public override Rarities Rarity => Rarities.Common;

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
