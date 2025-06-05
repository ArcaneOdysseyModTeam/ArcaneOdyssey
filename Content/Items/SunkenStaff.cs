using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOConversion;

namespace ArcaneOdyssey.Content.Items
{
	public class SunkenStaff : ModItem
    {
        public float AOSpeed = .9f;
        public float AOSize = 1.25f;
        public float AODamage = 1f;
        public int AOValue = 1350;
        public int AORarity = AORarities.Rare;
        public int AOWeaponTier = AOWeaponTiers.Excellent;

        public override void SetDefaults()
        {
            Item.DefaultToSpear(ModContent.ProjectileType<SunkenStaffProjectile1>(), WeaponSpeed(AOSpeed, AOWeaponTier), WeaponSpeed(AOSpeed, AOWeaponTier));
            Item.damage = WeaponDamage(AODamage, AOWeaponTier);
            Item.width =  40;
            Item.height = 40;
            Item.knockBack = WeaponSize(AOSize, AOWeaponTier);
            Item.rare = AORarity;
            Item.value = GalleonToCopper(AOValue, Item.rare);
            Item.UseSound = SoundID.SplashWeak;
            Item.autoReuse = true;
        }
		public override bool AltFunctionUse(Player player) => true;

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
			target.AddBuff(BuffID.Wet, 600);
            base.OnHitNPC(player, target, hit, damageDone);
        }

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.MonkStaffT3);
            recipe.AddIngredient<ArcaniumScrap>(2);
            recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}

        public override bool CanShoot(Player player)
        {
            return player.altFunctionUse == 2;
        }

        public override bool? UseItem(Player player)
		{ 
			if (player.altFunctionUse == 2 && !player.HasBuff<RisenTide>())
			{
				
            }
			return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
    }
}
