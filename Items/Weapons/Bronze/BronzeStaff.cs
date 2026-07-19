using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Materials;
using ArcaneOdyssey.Items.Weapons.Old;
using ArcaneOdyssey.Projectiles.Weapons;

namespace ArcaneOdyssey.Items.Weapons.Bronze
{
	public class BronzeStaff : Weapon
	{
		public override float Speed => 1;
		public override float Size => .9f;
		public override float Damage => 1.1f;
		public override int Value => 50;
		public override ItemRarities Rarity => ItemRarities.Uncommon;
		public override ItemTiers WeaponTier => ItemTiers.Average;
		public override Color Motif => Color.Orange;


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

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArcaneOdysseyMod.Sets.staff[Type] = true;
		}
	}
}
