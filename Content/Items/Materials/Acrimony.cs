using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Materials
{
    public class Acrimony : ModItem
    {
        public int AOValue = 10000;

        public AORarities AORarity = AORarities.Legendary;
        public override void SetDefaults()
        {
            Item.rare = (int)AORarity;
            Item.value = GalleonToCopper(AOValue);
        }

        public override void SetStaticDefaults()
        {
            ItemID.Sets.CanGetPrefixes[Type] = false;
        }

        public override void AddRecipes()
        {
        }
    }
}
