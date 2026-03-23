using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Materials;
using ArcaneOdyssey.Items.Weapons.Bronze;
using ArcaneOdyssey.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Weapons.Sunken
{
	public class SunkenStaff : Weapon
	{
		public override bool? Cold => true;
		public override float AOSpeed => .9f;
		public override float AOSize => 1.25f;
		public override float AODamage => 1f;
		public override int AOValue => 1350;
		public override AORarities AORarity => AORarities.Rare;
		public override AOItemTiers AOWeaponTier => AOItemTiers.Good;
		public override Debuff? WeaponDebuff => Debuff.Create<Soaked>();
		public override Color Motif => Color.Aqua;
		public override SoundStyle UseSound => SoundID.SplashWeak;


		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DamageType = DamageClass.MeleeNoSpeed;
			Item.shoot = ModContent.ProjectileType<SunkenStaffProjectile>();
			Item.width = Item.height = 60;
			Item.channel = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.reuseDelay = 120;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient<BronzeStaff>();
			recipe.AddIngredient<SunkenScrap>(2);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}
	}
}
