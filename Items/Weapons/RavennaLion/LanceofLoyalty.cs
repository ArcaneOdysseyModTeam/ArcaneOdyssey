using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Base;
using ArcaneOdyssey.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Weapons.RavennaLion
{
	public class LanceofLoyalty : Weapon
	{
		public override int Value => 200;
		public override WeaponType WeaponsType => WeaponType.Strength;
		public override Color Motif => Color.Gold;

		public override ItemTiers WeaponTier => ItemTiers.Good;

		public override Rarities Rarity => Rarities.Rare;
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
		}

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] < 1;

	}
}
