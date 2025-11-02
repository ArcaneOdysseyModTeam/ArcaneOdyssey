using ArcaneOdyssey.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Base
{
	public abstract class TechniqueScroll : EmptyScroll, ILocalizedModType
    {
        public override string LocalizationCategory => "FightingStyles.Scrolls";
        public override void UpdateInventory(Player player)
		{
			var imbue = Item.ArcaneOdyssey().Imbue;
            if (imbue is FightingStyle and not FightingStyleBarred)
            {
                Item.color = imbue.ImbueColour with { A = (byte)(255 * .9f) };
            }
            else if (imbue is FightingStyleBarred barred)
            {
                Item.color = Color.Lerp(Color.Transparent, barred.ImbueColour, barred.BarValue / FightingStyleBarred.BarMax);
            }
            else Item.color = Color.Transparent;
		}

		public override bool CanUseItem(Player player)
		{
			return Item.ArcaneOdyssey().Imbue is FightingStyle;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            AOPlayer playah = player.ArcaneOdyssey();
            Item.ArcaneOdyssey().Imbue = playah.Imbue;
            if (playah.Imbue is FightingStyle and not FightingStyleBarred)
            {
                Item.color = playah.Imbue.ImbueColour with { A = (byte)(255 * .9f) };
            }
            else if (playah.Imbue is FightingStyleBarred barred)
            {
                Item.color = Color.Lerp(Color.Transparent, barred.ImbueColour with { A = (byte)(255 * .9f) }, barred.BarValue / FightingStyleBarred.BarMax);
            }
            else Item.color = Color.Transparent;

        }
    }
}
