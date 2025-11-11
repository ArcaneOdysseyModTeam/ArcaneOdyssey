using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Items.Weapons.Old;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Weapons.Bronze
{
	public class BronzeTrident : AORangedOrMeleeWeapon
	{
		public override AOItemTiers AOWeaponTier => AOItemTiers.Average;
		public override AORarities AORarity => AORarities.Uncommon;
		public override float AODamage => 1.05f;
		public override float AOSize => 1;
		public override float AOSpeed => .95f;
		public override int AOValue => 50;
		public override WeaponAbility? Ability => new(Mod, "Trident Throw", "Throw your weapon, leaving yourself unarmed", Color.Orange);


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
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, ai2: player.AltUse() ? 1 : 0);
			return false;
		}

		public override bool AltFunctionUse(Player player)
		{
			return CanUseItem(player);
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BronzeBar>(25).AddIngredient(ItemID.Trident).AddTile(TileID.Anvils).Register();
            CreateRecipe().AddIngredient<BronzeBar>(25).AddIngredient(ItemID.Spear).AddTile(TileID.Anvils).Register();
        }
	}
}
