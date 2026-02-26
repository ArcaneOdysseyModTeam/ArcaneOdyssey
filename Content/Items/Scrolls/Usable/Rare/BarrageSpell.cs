using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Scrolls.Usable.Rare
{
	public class BarrageSpell : RareScroll
	{
		public override bool CanHaveMagic => true;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 5;
			Item.mana = 5;
			Item.DamageType = DamageClass.Magic;
			Item.shootSpeed = 7;
			Item.channel = true;
			Item.useTime = Item.useAnimation = 10;
			Item.shoot = ProjectileID.WoodenArrowFriendly; // does not actually shoot
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			AOMagic.CreateMagicCircle(Item, player, Imbue, damage);
			return false;
		}

		public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			mult = ApplyScrollSpeed(mult, true);
		}
	}
}
