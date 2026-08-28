using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Weapons;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Items.Weapons
{
	public class GildedMusket : Weapon
	{
		public override ItemTiers WeaponTier => ItemTiers.Average;

		public override Color Motif => Color.Gold;

		public override ItemRarities Rarity => ItemRarities.Uncommon;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArcaneOdysseyMod.Sets.weaponType[Type] = WeaponType.Strength;
			ItemID.Sets.gunProj[Type] = true;
			ItemID.Sets.IsRangedSpecialistWeapon[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 75;
			Item.DamageType = DamageClass.Generic;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<GildedMusketProjectile>();
			Item.useAmmo = AmmoID.Bullet;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shootSpeed = 1f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			var proj = Projectile.NewProjectileDirect(source, position, velocity, Item.shoot, damage, knockback, player.whoAmI, ai2: player.AltUse() ? type : 0);
			return false;
		}

		public override bool AltFunctionUse(Player player) => player.GetModPlayer<GildedPlayer>().BarProgress >= 1f;

		public override bool CanConsumeAmmo(Item ammo, Player player) => player.AltUse();

		public override bool NeedsAmmo(Player player) => player.AltUse();

		public override bool CanShoot(Player player) => player.ownedProjectileCounts[Item.shoot] < 1;

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient(ItemID.Bone, 30).AddIngredient(ItemID.GoldBar, 20).AddIngredient(ItemID.RichMahogany, 50).Register();
		}
	}

	public class GildedPlayer : ModPlayer
	{
		private float barProgress;

		public float BarProgress { get => barProgress; set => barProgress = value.Clamp(0, 1f); }

		public byte swingCount = 0;
	}
}
