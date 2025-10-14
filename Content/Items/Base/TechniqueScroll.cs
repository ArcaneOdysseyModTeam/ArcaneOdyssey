using ArcaneOdyssey.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Base
{
	public abstract class TechniqueScroll : EmptyScroll
	{
		public override void UpdateInventory(Player player)
		{
			var imbue = Item.ArcaneOdyssey().imbue;
            if (imbue is FightingStyle and not FightingStyleBarred)
            {
                Item.color = imbue.ImbueColour;
            }
            else if (imbue is FightingStyleBarred barred)
            {
                Item.color = Color.Lerp(Color.Transparent, barred.ImbueColour, barred.BarValue / 100);
            }
            else Item.color = Color.Transparent;
		}

		public override bool CanUseItem(Player player)
		{
			return Item.ArcaneOdyssey().imbue is FightingStyle;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            AOPlayer playah = player.ArcaneOdyssey();
            Item.ArcaneOdyssey().imbue = playah.imbue;
            if (playah.imbue is FightingStyle and not FightingStyleBarred)
            {
                Item.color = playah.imbue.ImbueColour;
            }
            else if (playah.imbue is FightingStyleBarred barred)
            {
                Item.color = Color.Lerp(Color.Transparent, barred.ImbueColour, barred.BarValue/100);
            }
            else Item.color = Color.Transparent;

        }
    }
}
