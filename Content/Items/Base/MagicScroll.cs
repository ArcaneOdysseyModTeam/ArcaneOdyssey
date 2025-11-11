using ArcaneOdyssey.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Base
{
	public abstract class MagicScroll : EmptyScroll, ILocalizedModType
	{
        public override string LocalizationCategory => "Magic.Scrolls";
        public override void UpdateInventory(Player player)
		{
			var imbue = Item.ArcaneOdyssey().Imbue;
			if (imbue is AOMagic)
			{
				Item.color = imbue.GetColor() with { A = (byte)(255 * .75f) };
			}
			else Item.color = Color.Transparent;
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            AOPlayer playah = player.ArcaneOdyssey();
            Item.ArcaneOdyssey().Imbue = playah.Imbue;
            if (playah.Imbue is AOMagic)
            {
                Item.color = playah.Imbue.GetColor() with { A = (byte)(255 * .75f) };
            }
            else Item.color = Color.Transparent;
        }

        public override bool CanUseItem(Player player)
		{
			return Item.ArcaneOdyssey().Imbue is AOMagic;
		}
	}
}
