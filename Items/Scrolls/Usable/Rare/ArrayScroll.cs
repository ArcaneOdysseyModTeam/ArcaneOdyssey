using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles;
using ArcaneOdyssey.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Scrolls.Usable.Rare
{
	public class ArrayScroll : RareScroll
	{
		public override bool CanHaveMagic => true;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 190;
			Item.mana = 50;
			Item.useTime = Item.useAnimation = 40;
			Item.DamageType = DamageClass.Magic;
			Item.shoot = ModContent.ProjectileType<ArraySpell>();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Imbuable.CreateMagicCircle(Item, player, MagicCircleMode.Basic, true, type, position: player.Top, rotation: -MathHelper.PiOver2);
			return false;
		}

		public override bool CanUseItem(Player player) => base.CanUseItem(player) && player.ownedProjectileCounts[Item.shoot] < 1;
	}
}
