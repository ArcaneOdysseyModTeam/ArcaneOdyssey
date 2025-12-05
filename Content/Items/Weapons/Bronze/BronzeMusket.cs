using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Weapons.Bronze
{
	public class BronzeMusket : AORangedOrMeleeWeapon
	{
		public override int AOValue => 140;
		public override AOItemTiers AOWeaponTier => AOItemTiers.Average;
		public override float AOSpeed => .5f;
		public override float AODamage => 1.5f;
		public override SoundStyle UseSound => SoundID.Item11;

		public override WeaponAbility? Ability => new(Mod, "Piercing Shot", "Converts Musket Balls to High Velocity Bullets", Color.Orange);

		public override AORarities AORarity => AORarities.Uncommon;
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
			if (type == ProjectileID.Bullet)
			{
				type = ProjectileID.BulletHighVelocity;
			}
        }

        public override Vector2? HoldoutOffset()
        {
            return new(-11, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.Musket).AddIngredient<BronzeBar>(10).Register();
		}
	}
}
