using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Weapons.Scrolls
{
	public class BeamScroll : Scroll
	{
		public override bool CanHaveMagic => true;
		public override int AOValue => 1000;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 100;
			Item.channel = true;
			Item.mana = 30;
			Item.knockBack = 0f;
			Item.DamageType = DamageClass.Magic;
			Item.shoot = ProjectileID.WoodenArrowFriendly; // does not actually shoot
		}

		public override void AddRecipes()
		{
			AddRecipe(ItemID.SpaceGun);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			AOMagic.CreateMagicCircle(Item, player, Imbue, damage);
			return false;
		}
	}
}
