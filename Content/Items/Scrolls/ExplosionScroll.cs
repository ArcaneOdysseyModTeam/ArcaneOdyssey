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
	public class ExplosionScroll : DefaultScroll
	{
		public override void SetDefaultsScroll()
		{
			Item.useAnimation = Item.useTime = 60*5;
			Item.damage = 25;
			Item.channel = true;
			Item.UseSound = SoundID.Item84;
			Item.mana = 50;
			Item.shootSpeed = 15;
			Item.shoot = ModContent.ProjectileType<ExplosionTracker>();
		}

        public override void ScrollRecipe()
        {
			CreateRecipe().AddIngredient<DefaultScroll>().AddIngredient(ItemID.Dynamite, 32).Register();
        }
        
        public override bool CanUseItem(Player player)
		{
			return player.AOPlayer().imbue is not null;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            AOPlayer playah = player.AOPlayer();
            AOMagic.CreateMagicCircle(Item, player);
            Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback * 1.5f, player.whoAmI);
			return false;
        }
	}
}
