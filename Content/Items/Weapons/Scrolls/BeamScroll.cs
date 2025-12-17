using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
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
			Item.damage = 20;
			Item.channel = true;
			Item.mana = 30;
			Item.DamageType = DamageClass.Magic;
			Item.shoot = ProjectileID.WoodenArrowFriendly; // does not actually shoot
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<EmptyScroll>().AddIngredient(ItemID.SpaceGun).Register();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			AOMagic.CreateMagicCircle(Item, player, Item.ArcaneOdyssey().Imbue);
			return false;
		}
	}
}
