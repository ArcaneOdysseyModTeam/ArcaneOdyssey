using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Materials;
using Terraria.Audio;

namespace ArcaneOdyssey.Items.Weapons.Bronze
{
	public class BronzeMusket : Weapon
	{
		public override int Value => 140;
		public override ItemTiers WeaponTier => ItemTiers.Average;
		public override float Speed => .8f;
		public override float Damage => 1.1f;
		public override SoundStyle UseSound => SoundID.Item11; // PORT change to 133

		public override Color Motif => Color.Orange;

		public override ItemRarities Rarity => ItemRarities.Uncommon;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = 66;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.height = 18;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.useAmmo = AmmoID.Bullet;
			Item.shootSpeed = 8;
			Item.shoot = ProjectileID.Bullet;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (type == ProjectileID.Bullet && !player.ArcaneOdyssey().OnCooldown<PiercingShotCooldown>())
			{
				player.ArcaneOdyssey().SetCooldown<PiercingShotCooldown>();
				ActivateAbility(player, true);
				type = ProjectileID.BulletHighVelocity;
				damage += new Item(ItemID.HighVelocityBullet).damage - new Item(ItemID.MusketBall).damage;
				velocity = velocity.Add(ApplySpeed(new Item(ItemID.HighVelocityBullet).shootSpeed - new Item(ItemID.MusketBall).shootSpeed));
			}
		}

		public override Vector2? HoldoutOffset()
		{
			return new(-11, 0);
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient(ItemID.Musket).AddIngredient<BronzeBar>(10).AddTile(TileID.Anvils).Register();
		}
	}

	public class PiercingShotCooldown : DisplayedCooldown
	{
		public override string Texture => AOUtils.GetTexture<BronzeMusket>();

		public override int CooldownLength => 90;
	}
}
