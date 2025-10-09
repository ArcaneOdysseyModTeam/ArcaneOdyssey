using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Armour.Centurion
{
    [AutoloadEquip(EquipType.Head)]
    public class RavennaHelm : AOArmour
	{
		public override int AODefense => 188;
		public override int AOSize => AODefense / 20;
		public override int AOAttkSpd => AODefense / 20;
		public override AORarities AORarity => AORarities.Uncommon;
        public override int AOValue => 83;

		public override void SetDefaults()
		{
			base.SetDefaults();
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<BronzeBar>(40).AddTile(TileID.Anvils).Register();
        }
    }
}
