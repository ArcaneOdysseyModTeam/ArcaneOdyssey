using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Weapons
{
	public class ScimitarofStorm : AORangedOrMeleeWeapon
	{
		public override int AOValue => 210;

		public override AOItemTiers AOWeaponTier => AOItemTiers.Average;

		public override AORarities AORarity => AORarities.Rare;

		public override float AOSpeed => 1.15f;
		public override float AODamage => 1.05f;
		public override float AOSize => .85f;


		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useTime = Item.useAnimation / 2;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noUseGraphic = true;
			Item.DamageType = DamageClass.MeleeNoSpeed;
			Item.shoot = ModContent.ProjectileType<ScimitarofStormProjectile>();
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			damage /= 2;
			knockback /= 2;
		}

		public override WeaponAbility? Ability => new(Mod, "Twin Crescents", "Slash both blades one after the other, sending two flying slashes towards the target", Color.Gold);

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}
	}
}
