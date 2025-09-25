using ArcaneOdyssey.Content.Items.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Materials
{
    public class ArcaniumScrap : AOBaseItem
    {
        public int AOValue = 400;
        public override AORarities AORarity => AORarities.Rare;
		public override ItemType ItemType => ItemType.Material;

		public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 9999;
            Item.rare = (int)AORarity;
            Item.value = GalleonToCopper(AOValue);
        }
    }
}
