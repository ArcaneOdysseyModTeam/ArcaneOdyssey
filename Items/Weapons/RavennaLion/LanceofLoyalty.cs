using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Base;
using ArcaneOdyssey.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Weapons.RavennaLion
{
	public class LanceofLoyalty : AOWeapon
	{
		public override int AOValue => 200;
		public override WeaponType WeaponsType => WeaponType.Strength;
		public override Color Colour => Color.Gold;

		public override AOItemTiers AOWeaponTier => AOItemTiers.Good;

		public override AORarities AORarity => AORarities.Rare;
		public override float AOSpeed => .675f;
		public override float AOSize => 1.25f;
		public override float AODamage => 1.1f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.width = Item.height = 60;
			Item.StopAnimationOnHurt = true;
			Item.channel = true;
			Item.DamageType = AOUtils.TrueMeleeNoSpeed();
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.shoot = ModContent.ProjectileType<LanceofLoyaltyProjectile>();
			Item.shootSpeed = BaseLanceProjectile.Speed;
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<LionsHalberd>();
		}

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] < 1;
	}
}
