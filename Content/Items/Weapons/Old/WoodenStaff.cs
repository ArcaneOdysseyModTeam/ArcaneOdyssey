using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using ArcaneOdyssey.Content.Buffs;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.CodeAnalysis.Operations;
using ArcaneOdyssey.Content.Projectiles.Weapons;
using ArcaneOdyssey.Content.Items.Materials;
using Terraria.Audio;

namespace ArcaneOdyssey.Content.Items.Weapons.Old
{
    public class WoodenStaff : AORangedOrMeleeWeapon
    {
        public override float AOSpeed => 1.05f;
        public override float AOSize => 0.9f;
        public override float AODamage => 1f;
        public override int AOValue => 1350;
        public override AORarities AORarity => AORarities.Common;
        public override AOItemTiers AOWeaponTier => AOItemTiers.Poor;
        public override AODebuffRequirement? WeaponDebuff => null; // dull weapon
        public override SoundStyle UseSound => SoundID.Item1;


		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DamageType = TrueMeleeNoSpeed();
			Item.shoot = ModContent.ProjectileType<WoodenStaffProjectile>();
            Item.width = Item.height = 60;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.noUseGraphic = true;
			Item.reuseDelay = 120;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.AshWood, 32);
			recipe.AddRecipeGroup(RecipeGroupID.IronBar, 12);
			recipe.AddTile(TileID.Hellforge);
            recipe.Register();
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}
	}
}
