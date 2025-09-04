using ArcaneOdyssey.Content.Mounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ID;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Equipment
{
	public class WoodenPaddle : ModItem
	{
		public int AORarity = AORarities.Common;
		public override void SetDefaults()
		{
			Item.width = Item.height = 80;
			Item.mountType = ModContent.MountType<Rowboat>();
			Item.value = GalleonToCopper(10, AORarity);
		}
        public override void AddRecipes()
        {
			CreateRecipe().AddIngredient(ItemID.PalmWood, 100).AddTile(TileID.WorkBenches).Register();
        }
	}
}
