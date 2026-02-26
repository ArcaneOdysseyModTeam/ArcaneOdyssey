using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Items.Weapons.Old;
using ArcaneOdyssey.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Weapons.Bronze
{
	public class BronzeStaff : AORangedOrMeleeWeapon
	{
		public override float AOSpeed => 1;
		public override float AOSize => .9f;
		public override float AODamage => 1.1f;
		public override int AOValue => 50;
		public override AORarities AORarity => AORarities.Uncommon;
		public override AOItemTiers AOWeaponTier => AOItemTiers.Average;
		public override WeaponAbility? Ability => new(this, Color.Orange);


		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DamageType = DamageClass.MeleeNoSpeed;
			Item.shoot = ModContent.ProjectileType<BronzeStaffProjectile>();
			Item.width = Item.height = 64;
			Item.channel = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.reuseDelay = 120;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BronzeBar>(12).AddIngredient<WoodenStaff>().AddTile(TileID.Anvils).Register();
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}
	}
}
