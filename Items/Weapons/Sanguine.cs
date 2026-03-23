using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Abilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Weapons
{
	public class Sanguine : Weapon
	{
		public override int AOValue => 125;

		public override AOItemTiers AOWeaponTier => AOItemTiers.Poor; // unfortunately a pre boss item

		public override Color Motif => Color.Red;

		public override AORarities AORarity => AORarities.Uncommon;

		public override float AODamage => 1.1f;
		public override float AOSpeed => 1.05f;
		public override float AOSize => .85f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.height = Item.width = 44;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shoot = ModContent.ProjectileType<SanguineThrow>();
			Item.shootSpeed = 11f;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.autoReuse = true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			ActivateAbility(player, true);
			return true;
		}
	}
}
