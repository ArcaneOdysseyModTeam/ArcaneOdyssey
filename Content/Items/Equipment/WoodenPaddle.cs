using ArcaneOdyssey.Content.Mounts;
using Terraria.ModLoader;
using Terraria.ID;
using ArcaneOdyssey.Content.Items.Base;

namespace ArcaneOdyssey.Content.Items.Equipment
{
	public class WoodenPaddle : AOBaseItem, ILocalizedModType
	{
		public override string LocalizationCategory => base.LocalizationCategory + ".Mounts";
		public override AORarities AORarity => AORarities.Common;

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
