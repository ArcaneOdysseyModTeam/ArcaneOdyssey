using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Items.Weapons.Old;
using ArcaneOdyssey.Content.Projectiles.Weapons.Abilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Weapons.Bronze
{
	public class RavennaGreatsword : AORangedOrMeleeWeapon
	{
		public override int AOValue => 40;
		public override float AOSize => 1.05f;
		public override float AOSpeed => .9f;
		public override float AODamage => 1.05f;
		public override AORarities AORarity => AORarities.Uncommon;
		public override AOItemTiers AOWeaponTier => AOItemTiers.Average;
		public override WeaponAbility? Ability => new(Mod, "Mountain Wind", "Swing your blade and unleash three tornados that spread out", Color.Orange);

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.height = Item.height = 64;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shoot = ModContent.ProjectileType<MountainWind>();
			Item.shootSpeed = 5;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BronzeBar>(12).AddIngredient<OldGreatsword>().AddTile(TileID.Anvils).Register();
		}

        public bool EveryOther = true;
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            if (EveryOther)
            {
                float anglediv = 9;
                var angle1 = velocity.ToRotation() + MathHelper.Pi / anglediv;
                var angle2 = velocity.ToRotation() - MathHelper.Pi / anglediv;
                Projectile.NewProjectile(source, position, angle1.ToRotationVector2() * Item.shootSpeed * (Imbue?.AOImbueSpeed ?? 1f), type, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, angle2.ToRotationVector2() * Item.shootSpeed * (Imbue?.AOImbueSpeed ?? 1f), type, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            }
            EveryOther = !EveryOther;
            return false;
		}
	}
}
