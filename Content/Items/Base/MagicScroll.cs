using ArcaneOdyssey.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;

namespace ArcaneOdyssey.Content.Items.Base
{
	public abstract class MagicScroll : EmptyScroll
	{
		public override void UpdateInventory(Player player)
		{
			var imbue = Item.ArcaneOdyssey().imbue;
			if (imbue is AOMagic)
			{
				Item.color = imbue.ImbueColour;
			}
			else Item.color = Color.Transparent;
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            AOPlayer playah = player.ArcaneOdyssey();
            Item.ArcaneOdyssey().imbue = playah.imbue;
            if (playah.imbue is AOMagic)
            {
                Item.color = playah.imbue.ImbueColour;
            }
            else Item.color = Color.Transparent;
        }

        public override bool CanUseItem(Player player)
		{
			return Item.ArcaneOdyssey().imbue is AOMagic;
		}
	}
}
