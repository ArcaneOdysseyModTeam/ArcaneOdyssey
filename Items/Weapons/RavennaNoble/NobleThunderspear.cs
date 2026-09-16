using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Base;
using ArcaneOdyssey.Projectiles.Weapons;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Items.Weapons.RavennaNoble
{
	public class NobleThunderspear : Weapon
	{
		public override int Value => 135;

		public override ItemTiers WeaponTier => ItemTiers.Average;

		public override ItemRarities Rarity => ItemRarities.Uncommon;

		public override float Damage => 1.05f;

		public override float Speed => 1.15f;

		public override float Size => .85f;

		public override Color Motif => Color.MediumPurple;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.Spears[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.shoot = ModContent.ProjectileType<NobleThunderspearProjectile>();
			Item.shootSpeed = BaseSpearProjectile.SpearSpeed;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.autoReuse = true;
			Item.height = 70;
			Item.width = 72;
		}

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] < 1;

		public override bool AltFunctionUse(Player player) => !player.ArcaneOdyssey().OnCooldown<SparrowThrustCooldown>();

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, ai2: player.altFunctionUse);
			return false;
		}
	}

	public class SparrowThrustCooldown : DisplayedCooldown
	{
		public override int CooldownLength => 60 * 5;
		public override string Texture => AOUtils.GetTexture<NobleThunderspear>();
	}
}
