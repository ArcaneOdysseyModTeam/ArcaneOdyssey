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
		public override bool SpellScroll => false;

		public override void UpdateInventory(Player player)
		{
			AOPlayer playah = player.ArcaneOdyssey();
			if (playah.imbue is FightingStyle)
			{
				Item.color = playah.imbue.ImbueColour;
				if (Item.color == Color.White || Item.color == Color.Black)
				{
					Item.color.A *= (byte).5f;
				}
			}
			else Item.color = default;
		}

		public override bool CanUseItem(Player player)
		{
			return player.ArcaneOdyssey().imbue is FightingStyle;
		}
	}
}
