using ArcaneOdyssey.Content.Items.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Materials
{
    public class HecateOrb : AOBaseItem
    {
        public int AOValue = 20000;
        public override AORarities AORarity => AORarities.Arcane;
		public override ItemType ItemType => ItemType.Material;

		public override void SetDefaults()
        {
            Item.rare = (int)AORarity;
            Item.value = GalleonToCopper(AOValue);
        }
        public override void SetStaticDefaults()
        {
            ItemID.Sets.CanGetPrefixes[Type] = false;
            ItemID.Sets.ShimmerTransformToItem[Type] = Type;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }
    }
}
