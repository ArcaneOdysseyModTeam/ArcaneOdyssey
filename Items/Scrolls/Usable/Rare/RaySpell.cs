using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Scrolls.Usable.Rare
{
	public class RaySpell : RareScroll
	{
		public override bool CanHaveMagic => true;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.mana = 12;
			Item.DamageType = DamageClass.Magic;
			Item.shootSpeed = 7f;
			Item.channel = true;
			Item.damage = Item.useTime = Item.useAnimation = 5;
			Item.shoot = ProjectileID.WoodenArrowFriendly; // does not actually shoot
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			AOMagic.CreateMagicCircle(Item, player, Imbue, damage);
			return false;
		}

		public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			mult = ApplySpeed(mult, true);
		}
	}
}
