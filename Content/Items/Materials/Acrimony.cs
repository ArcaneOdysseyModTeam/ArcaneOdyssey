using ArcaneOdyssey.Content.Items.Base;
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
    public class Acrimony : AOBaseItem
    {
        public int AOValue = 10000;

		public override ItemType ItemType => ItemType.Material;
		public override AORarities AORarity => AORarities.Arcane;
        public override void SetDefaults()
		{
			base.SetDefaults();
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
