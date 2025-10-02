using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using ArcaneOdyssey.Content.Buffs;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.CodeAnalysis.Operations;
using ArcaneOdyssey.Content.Projectiles.Weapons;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Items.Weapons.Old;

namespace ArcaneOdyssey.Content.Items.Weapons.Bronze
{
	public class BronzeStaff : AORangedOrMeleeWeapon
	{
		public override float AOSpeed => 1;
		public override float AOSize => .9f;
		public override float AODamage => 1.1f;
		public override int AOValue => 50;
		public override AORarities AORarity => AORarities.Uncommon;
		public override AOWeaponTiers AOWeaponTier => AOWeaponTiers.Average;


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
			Recipe recipe = CreateRecipe();
			CreateRecipe().AddIngredient<BronzeBar>(25).AddIngredient<WoodenStaff>().AddTile(TileID.Anvils).Register();
			recipe.Register();
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}
	}
}
