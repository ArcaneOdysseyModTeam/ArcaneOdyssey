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

namespace ArcaneOdyssey.Content.Items.Armour.Sunken
{
    [AutoloadEquip(EquipType.Body)]
    public class SunkenChest : AOArmour
    {
        public override int AODefense => 204;
        public override int AOSize => 23;
        public override int AOAttkSpd => 23;
        public override int AORarity => AORarities.Rare;

        public override int AOValue => 2700;

        public override int MinionSlots => 2;

        public override void SetDefaultsArmour()
        {
            Item.width = Item.height = 48;
        }

        public override void UpdateArmour(Player player)
        {
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<ArcaniumScrap>(5).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}
