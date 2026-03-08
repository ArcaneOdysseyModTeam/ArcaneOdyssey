using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Abilities;
using ArcaneOdyssey.PlayerClasses;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Weapons
{
	public class Sinbane : AOWeapon
	{
		public override int AOValue => 400;
		public override AORarities AORarity => AORarities.Uncommon;
		public override AOItemTiers AOWeaponTier => AOItemTiers.Good;
		public override float AOSpeed => 1.1f;
		public override float AOSize => .8f;
		public override float AODamage => 1.1f;

		public override Color Colour => new(104, 130, 0);

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useStyle = ItemUseStyleID.Swing;
			Item.DamageType = AOUtils.TrueMelee();
			Item.width = Item.height = 76;
			Item.shoot = ModContent.ProjectileType<ToweringImpact>();
			Item.shootSpeed = 3f;
		}

		public override bool AltFunctionUse(Player player) => !player.ArcaneOdyssey().OnCooldown<ToweringImpactCooldown>();

		public override bool CanShoot(Player player) => player.AltUse();

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			ActivateAbility(player, false);
			player.ArcaneOdyssey().SetCooldown<ToweringImpactCooldown>();
			return true;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			knockback *= 3f;
		}
	}

	public class ToweringImpactCooldown : DisplayedCooldown
	{
		public override string ExtraIconTexture => AOUtils.GetTexture<Sinbane>();

		public override int CooldownLength => 60 * 5;
	}
}
