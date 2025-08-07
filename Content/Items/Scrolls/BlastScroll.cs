using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
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

		public override bool CanUseItem(Player player)
		{
			return player.GetModPlayer<AOPlayer>().imbue is not null;
		}

		public override bool? UseItem(Player player)
		{
			AOPlayer playah = player.GetModPlayer<AOPlayer>();
			AOMagic magic = playah.imbue;
			Projectile.NewProjectile(player.GetSource_FromThis(), player.itemLocation, player.itemRotation.ToRotationVector2() * 20, magic.Spells[typeof(BlastSpell)], Item.damage, Item.knockBack);
			return true;
        }
	}
}
