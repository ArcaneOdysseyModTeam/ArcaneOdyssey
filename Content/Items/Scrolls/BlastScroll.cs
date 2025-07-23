using ArcaneOdyssey.Content.Items.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Scrolls
{
    public class BlastScroll : DefaultScroll
    {
        public override void SetDefaultsScroll()
        {
            Item.useTime = 15;
            Item.useAnimation = 60;
            Item.damage = 10;
        }
    }
}
