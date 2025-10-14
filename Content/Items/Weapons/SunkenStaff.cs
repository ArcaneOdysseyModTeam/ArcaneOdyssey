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
using ArcaneOdyssey.Content.Items.Weapons.Bronze;

namespace ArcaneOdyssey.Content.Items.Weapons
{
    public class SunkenStaff : AORangedOrMeleeWeapon
    {
        public override bool? ColdWeapon => true;
        public override float AOSpeed => .9f;
        public override float AOSize => 1.25f;
        public override float AODamage => 1f;
        public override int AOValue => 1350;
        public override AORarities AORarity => AORarities.Rare;
        public override AOItemTiers AOWeaponTier => AOItemTiers.Good;
        public override AODebuffRequirement? WeaponDebuff => new(BuffID.Wet, 600);
		public override WeaponAbility? Ability => new(Mod, "Fury of the Sea", "Shoots blasts of water that pierce enemies", Color.Aqua);


		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DamageType = DamageClass.MeleeNoSpeed;
			Item.shoot = ModContent.ProjectileType<SunkenStaffProjectile>();
            Item.width = Item.height = 60;
            Item.channel = true;
            Item.UseSound = SoundID.SplashWeak with { Pitch = AOSpeed.MultiToPercent().Clamp(-1, 1) };
            Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.reuseDelay = 120;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<BronzeStaff>();
            recipe.AddIngredient<SunkenScrap>(2);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}
	}
}
