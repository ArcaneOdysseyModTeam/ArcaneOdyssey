using ArcaneOdyssey.Content.Items.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static ArcaneOdyssey.AOUtils;
using Terraria.DataStructures;
using ArcaneOdyssey.Content.Projectiles.Weapons.Abilities;

namespace ArcaneOdyssey.Content.Items.Weapons
{
	public class ColossalGreatsword : AORangedOrMeleeWeapon
	{
		public override float AOSpeed => .65f;
		public override float AOSize => 1.2f;
		public override float AODamage => 1.15f;
		public override int AOValue => 250;
		public override AORarities AORarity => AORarities.Uncommon;
		public override AOItemTiers AOWeaponTier => AOItemTiers.Good;
		public override bool? Arcanium => false;
		public override WeaponAbility? Ability => new(Mod, "Colossal Cleave", "Unleash a large slash that pierces enemies", Color.PaleVioletRed);

		public override void SetStaticDefaults()
		{
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
			ItemID.Sets.UsesBetterMeleeItemLocation[Type] = true;
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
		public override bool CanShoot(Player player)
		{
			return player.AltUse() && !player.ArcaneOdyssey().OnCooldown(Name);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            player.ArcaneOdyssey().Cooldowns.Add(new ColossalCleaveCooldown().AOCooldown);
			Projectile.NewProjectile(source, position, Vector2.UnitX * Item.shootSpeed * player.direction, type, damage, knockback, player.whoAmI);
			return false;
		}

		public override bool AltFunctionUse(Player player)
		{
			return CanUseItem(player);
		}
	}

    public class ColossalCleaveCooldown : CooldownSystem
    {
        public override string Name => "Colossal Cleave Cooldown";
        public override int CooldownLength => 60 * 3;
        public override bool DisplayCooldown => true;
    }
}
