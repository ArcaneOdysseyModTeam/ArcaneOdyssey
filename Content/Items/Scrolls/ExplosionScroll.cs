using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Scrolls
{
	public class ExplosionScroll : EmptyMagicScroll
	{
		public override void SetDefaultsScroll()
		{
			Item.useAnimation = Item.useTime = ExplosionTracker.defaultMax-ExplosionTracker.defaultMin;
			Item.damage = 50;
			Item.reuseDelay = 60;
			Item.channel = true;
			Item.UseSound = SoundID.Item84;
			Item.mana = 100;
			Item.shoot = ModContent.ProjectileType<ExplosionTracker>();
		}

		public override void ScrollRecipe()
		{
			CreateRecipe().AddIngredient<EmptyMagicScroll>().AddIngredient(ItemID.Dynamite, 32).Register();
		}

		public override bool AltFunctionUse(Player player)
		{
			return CanUseItem(player);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			AOMagic.CreateMagicCircle(Item, player, player.Imbue());
			Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback * 1.5f, player.whoAmI);
			return false;
		}
	}
}
