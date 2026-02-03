using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Weapons;
using ArcaneOdyssey.PlayerClasses;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Weapons.RavennaNoble
{
	public class NobleThunderspear : AORangedOrMeleeWeapon
	{
		public override int AOValue => 135;

		public override AOItemTiers AOWeaponTier => AOItemTiers.Average;

		public override AORarities AORarity => AORarities.Rare;

		public override float AODamage => 1.05f;

		public override float AOSpeed => 1.15f;

		public override float AOSize => .85f;

		public override WeaponAbility? Ability => new(Mod, "Sparrow Thrust", "Use the blade and length of the spear to deliver a powerful thrust", Color.MediumPurple);

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.Spears[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.shoot = ModContent.ProjectileType<NobleThunderspearProjectile>();
			Item.shootSpeed = BaseSpearProjectile.Speed;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.autoReuse = true;
			Item.height = 70;
			Item.width = 72;
		}

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] < 1;

		public override bool AltFunctionUse(Player player) => !player.ArcaneOdyssey().OnCooldown<SparrowThrustCooldown>();

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, ai2: player.altFunctionUse + 1);
			return false;
		}
	}

	public class SparrowThrustCooldown : DisplayedCooldown
	{
		public override string ExtraIconTexture => AOUtils.GetTexture<NobleThunderspear>();
	}
}
