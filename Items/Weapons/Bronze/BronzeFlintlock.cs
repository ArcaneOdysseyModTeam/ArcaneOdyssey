using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Weapons.Bronze
{
	public class BronzeFlintlock : Weapon
	{
		public override int Value => 60;
		public override ItemTiers WeaponTier => ItemTiers.Average;
		public override SoundStyle UseSound => SoundID.Item11; // PORT change to 134

		public override Color Motif => Color.Orange;

		public override ItemRarities Rarity => ItemRarities.Uncommon;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = 36;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.height = 22;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.useAmmo = AmmoID.Bullet;
			Item.shootSpeed = 5;
			Item.shoot = ProjectileID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new(-2, 0);
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.AltUse() && !player.ArcaneOdyssey().OnCooldown<MultiShotCooldown>())
			{
				ActivateAbility(player, true);
				player.ArcaneOdyssey().SetCooldown<MultiShotCooldown>();
				for (int i = 0; i < 8; i++)
				{
					var offset = MathHelper.TwoPi / 16f;
					var velo = (velocity.ToRotation() - offset + (offset * 2 * Main.rand.NextFloat())).ToRotationVector2() * velocity.Length();
					Projectile.NewProjectile(source, position, velo, type, damage / 4, knockback / 4, player.whoAmI);
				}
				return false;
			}
			return true;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient(ItemID.TheUndertaker).AddIngredient<BronzeBar>(10).AddTile(TileID.Anvils).Register();
		}
	}

	public class MultiShotCooldown : DisplayedCooldown
	{
		public override string Texture => AOUtils.GetTexture<BronzeFlintlock>();

		public override int CooldownLength => 90;
	}
}
