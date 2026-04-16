using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Materials;
using ArcaneOdyssey.Items.Weapons.Old;
using ArcaneOdyssey.Projectiles.Abilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Weapons.Bronze
{
	public class RavennaGreatsword : Weapon
	{
		public override int Value => 40;
		public override float Size => 1.2f;
		public override float Speed => .9f;
		public override float Damage => 1.05f;
		public override Rarities Rarity => Rarities.Uncommon;
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

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			ActivateAbility(player, true);
			float anglediv = 9;
			var angle1 = velocity.ToRotation() + MathHelper.Pi / anglediv;
			var angle2 = velocity.ToRotation() - MathHelper.Pi / anglediv;
			Projectile.NewProjectile(source, position, angle1.ToRotationVector2() * velocity.Length(), type, damage, knockback, player.whoAmI);
			Projectile.NewProjectile(source, position, angle2.ToRotationVector2() * velocity.Length(), type, damage, knockback, player.whoAmI);
			return true;
		}

		public override bool CanShoot(Player player) => swings == 1;


		public int swings = 0;
		public int noUseCounter = 0;

		public override void UseAnimation(Player player)
		{
			noUseCounter = 0;
			if (++swings > 3)
			{
				swings = 0;
			}
		}

		public override void UpdateInventory(Player player)
		{
			if (!Main.mouseLeft && noUseCounter < 100)
			{
				noUseCounter++;
			}

			if (noUseCounter > 120 || player.PlayerItem().type != Type)
			{
				swings = 0;
			}
		}
	}
}
