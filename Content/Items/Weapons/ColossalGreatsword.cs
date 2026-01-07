using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Weapons.Abilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Weapons
{
	public class ColossalGreatsword : AORangedOrMeleeWeapon
	{
		public override float AOSpeed => .65f;
		public override float AOSize => 1.2f;
		public override float AODamage => 1.15f;
		public override int AOValue => 250;
		public override AORarities AORarity => AORarities.Rare;
		public override AOItemTiers AOWeaponTier => AOItemTiers.Good;
		public override WeaponType WeaponsType => WeaponType.Strength;
		public override WeaponAbility? Ability => new(Mod, "Colossal Cleave", "Unleash a large slash that pierces enemies", Color.PaleVioletRed);

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = 86;
			Item.shootSpeed = 5;
			Item.height = 86;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.shoot = ModContent.ProjectileType<ColossalCleave>();
		}

		/// <summary>
		/// only shoots projectile on alt fire
		/// </summary>
		/// <param name="player">the FUCKING PLAYER</param>
		/// <returns></returns>
		public override bool AltFunctionUse(Player player)
		{
			return Imbue is not null && !player.ArcaneOdyssey().OnCooldown(ModContent.BuffType<ColossalCleaveCooldown>());
		}

		public override bool CanShoot(Player player) => player.AltUse();

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			player.ArcaneOdyssey().SetCooldown(new ColossalCleaveCooldown());
			Projectile.NewProjectile(source, position, Vector2.UnitX * velocity.Length() * player.direction, type, damage, knockback, player.whoAmI);
			return false;
		}
	}

	public class ColossalCleaveCooldown : DisplayedCooldown
	{
		public override int CooldownLength => 60 * 3;
		public override string ExtraIconTexture => typeof(ColossalGreatsword).FullName.Replace('.', '/');
	}
}
