using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Weapons.Scrolls
{
	public class CannonScroll : MagicScroll
	{

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 5 * 9;
			Item.mana = 30;
			Item.DamageType = DamageClass.Magic;
			Item.shootSpeed = 7;
			Item.shoot = ProjectileID.WoodenArrowFriendly; // does not actually shoot
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BlastScroll>().AddIngredient(ItemID.FlowerofFire).Register();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			AOMagic.CreateMagicCircle(Item, player, Item.ArcaneOdyssey().imbue);
			return false;
		}
	}
}
