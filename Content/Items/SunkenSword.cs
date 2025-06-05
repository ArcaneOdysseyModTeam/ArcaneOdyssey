using ArcaneOdyssey.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOConversion;

namespace ArcaneOdyssey.Content.Items
{
	public class SunkenSword : ModItem
    {
        public float AOSpeed = 1.2f;
        public float AOSize = .9f;
        public float AODamage = 1f;
        public int AOValue = 900;
        public int AORarity = AORarities.Rare;
        public int AOWeaponTier = AOWeaponTiers.Excellent;

        public override void SetDefaults()
		{
			Item.damage = WeaponDamage(AODamage, AOWeaponTier);
			Item.DamageType = DamageClass.Melee;
			Item.width = 42;
			Item.height = 42;
			Item.useTime = WeaponSpeed(AOSpeed, AOWeaponTier);
			Item.useAnimation = WeaponSpeed(AOSpeed, AOWeaponTier);
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = WeaponSize(AOSize, AOWeaponTier);
            Item.rare = AORarity;
            Item.value = GalleonToCopper(AOValue, Item.rare);
			Item.UseSound = SoundID.SplashWeak;
			Item.autoReuse = true;
			Item.scale = WeaponSize(AOSize, AOWeaponTier);
        }
		public override bool AltFunctionUse(Player player) => true;

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
			target.AddBuff(BuffID.Wet, 600);
            base.OnHitNPC(player, target, hit, damageDone);
        }

        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (player.dashType != DashID.None && !player.HasBuff<RisenTide>())
			{
				modifiers.ScalingArmorPenetration = AddableFloat.Zero + 1f;
                player.AddBuff(ModContent.BuffType<RisenTide>(), 60 * 5);
            }
        }

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DD2SquireBetsySword);
            recipe.AddIngredient<ArcaniumScrap>(2);
            recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}

        public override bool? UseItem(Player player)
		{ 
			if (player.altFunctionUse == 2 && !player.HasBuff<RisenTide>())
			{
				player.DashMovement();
            }
			return true;
        }
    }
}
