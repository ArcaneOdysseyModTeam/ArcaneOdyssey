using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Materials;
using ArcaneOdyssey.Projectiles.Abilities;
using ArcaneOdyssey.Projectiles.Base;
using ArcaneOdyssey.Projectiles.Weapons.Bronze;

namespace ArcaneOdyssey.Items.Weapons.Bronze
{
	[LegacyName("BronzeTrident")]
	public class BronzeSpear : Weapon
	{
		public override ItemTiers WeaponTier => ItemTiers.Average;
		public override ItemRarities Rarity => ItemRarities.Uncommon;
		public override float Damage => 1.05f;
		public override float Size => 1;
		public override float Speed => .95f;
		public override int Value => 50;
		public override Color Motif => Color.Orange;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.Spears[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.shoot = ModContent.ProjectileType<BronzeSpearProjectile>();
			Item.shootSpeed = BaseSpearProjectile.SpearSpeed;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.autoReuse = true;
			Item.width = Item.height = 60;
		}

		public override bool CanUseItem(Player player)
		{
			if (!player.AltUse())
				Item.useStyle = ItemUseStyleID.Shoot;
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (player.AltUse() && !player.ArcaneOdyssey().OnCooldown<SpearThrowCooldown>())
			{
				type = ModContent.ProjectileType<SpearThrow>();
				velocity *= 3f;
				ActivateAbility(player, false);
				player.ArcaneOdyssey().SetCooldown<SpearThrowCooldown>();
			}
		}

		public override bool AltFunctionUse(Player player)
		{
			if (!player.ArcaneOdyssey().OnCooldown<SpearThrowCooldown>())
			{
				Item.useStyle = ItemUseStyleID.Swing;
				return true;
			}
			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BronzeBar>(10).AddIngredient(ItemID.Spear).AddTile(TileID.Anvils).Register();
		}
	}

	public class SpearThrowCooldown : DisplayedCooldown
	{
		public override string Texture => AOUtils.GetTexture<BronzeSpear>();

		public override int CooldownLength => 60;
	}
}
