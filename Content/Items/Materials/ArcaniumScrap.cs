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
			base.SetDefaults();
			Item.width = Item.height = 28;
            Item.maxStack = 9999;
			Item.value = GalleonToCopper(AOValue);
        }
    }
}
