using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Materials;
using ArcaneOdyssey.Projectiles.Base;
using ArcaneOdyssey.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Weapons.Bronze
{
	public class BronzeTrident : AOWeapon
	{
		public override AOItemTiers AOWeaponTier => AOItemTiers.Average;
		public override AORarities AORarity => AORarities.Uncommon;
		public override float AODamage => 1.05f;
		public override float AOSize => 1;
		public override float AOSpeed => .95f;
		public override int AOValue => 50;
		public override Color Colour => Color.Orange;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.Spears[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.shoot = ModContent.ProjectileType<BronzeTridentProjectile>();
			Item.shootSpeed = BaseSpearProjectile.Speed;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.autoReuse = true;
			Item.width = Item.height = 60;
		}

		public override bool CanUseItem(Player player)
		{
			if (!player.AltUse())
				Item.useStyle = ItemUseStyleID.Shoot;
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, ai2: player.altFunctionUse);
			return false;
		}

		public override bool AltFunctionUse(Player player)
		{
			Item.useStyle = ItemUseStyleID.Swing;
			ActivateAbility(player, false);
			return true;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BronzeBar>(10).AddIngredient(ItemID.Trident).AddTile(TileID.Anvils).Register();
			CreateRecipe().AddIngredient<BronzeBar>(10).AddIngredient(ItemID.Spear).AddTile(TileID.Anvils).Register();
		}
	}
}
