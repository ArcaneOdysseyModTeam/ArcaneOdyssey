using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Materials;
using ArcaneOdyssey.Items.Weapons.Old;
using ArcaneOdyssey.Projectiles.Abilities;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Items.Weapons.Bronze
{
	public class RavennaGreatsword : Weapon
	{
		public override int Value => 40;
		public override float Size => 1.2f;
		public override float Speed => .9f;
		public override float Damage => 1.05f;
		public override ItemRarities Rarity => ItemRarities.Uncommon;
		public override ItemTiers WeaponTier => ItemTiers.Average;
		public override Color Motif => Color.Orange;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.height = Item.height = 64;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shoot = ModContent.ProjectileType<MountainWind>();
			Item.shootSpeed = 5;
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArcaneOdysseyMod.Sets.greatsword[Type] = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BronzeBar>(12).AddIngredient<OldGreatsword>().AddTile(TileID.Anvils).Register();
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			damage = damage / 4 * 3;
		}

		public const float AngleDiv = 9f;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			player.ArcaneOdyssey().SetCooldown<MountainWindCooldown>();
			ActivateAbility(player, true);
			var angle1 = velocity.ToRotation() + MathHelper.Pi / AngleDiv;
			var angle2 = velocity.ToRotation() - MathHelper.Pi / AngleDiv;
			Projectile.NewProjectile(source, position, angle1.ToRotationVector2() * velocity.Length(), type, damage, knockback, player.whoAmI);
			Projectile.NewProjectile(source, position, angle2.ToRotationVector2() * velocity.Length(), type, damage, knockback, player.whoAmI);
			return true;
		}

		public override bool CanShoot(Player player) => !player.ArcaneOdyssey().OnCooldown<MountainWindCooldown>();
	}

	public class MountainWindCooldown : DisplayedCooldown
	{
		public override string Texture => AOUtils.GetTexture<RavennaGreatsword>();

		public override int CooldownLength => 120;
	}
}
