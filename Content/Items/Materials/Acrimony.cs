using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Materials
{
    public class Acrimony : ModItem
    {
        public int AOValue = 10000;

        public int AORarity = AORarities.Legendary;
        public override void SetDefaults()
        {
            Item.rare = AORarity;
            Item.value = GalleonToCopper(AOValue, AORarity);
            Item.maxStack = 2;
        }

        public override void AddRecipes()
        {
        }
    }
}
