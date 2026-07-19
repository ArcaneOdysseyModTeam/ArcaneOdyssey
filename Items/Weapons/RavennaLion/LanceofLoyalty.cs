using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Base;
using ArcaneOdyssey.Projectiles.Weapons;

namespace ArcaneOdyssey.Items.Weapons.RavennaLion
{
	public class LanceofLoyalty : Weapon
	{
		public override int Value => 200;
		public override Color Motif => Color.Gold;

		public override ItemTiers WeaponTier => ItemTiers.Good;

		public override ItemRarities Rarity => ItemRarities.Rare;
		public override float Speed => .675f;
		public override float Size => 1.25f;
		public override float Damage => 1.1f;

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
			Item.shootSpeed = BaseLanceProjectile.LanceSpeed;
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<LionsHalberd>();
			ArcaneOdysseyMod.Sets.weaponType[Type] = WeaponType.Strength;
		}

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] < 1;

	}
}
