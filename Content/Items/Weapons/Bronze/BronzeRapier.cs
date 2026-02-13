using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Items.Weapons.Old;
using ArcaneOdyssey.Content.Projectiles.Weapons.Abilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Weapons.Bronze
{
	public class BronzeRapier : AORangedOrMeleeWeapon
	{
		public override float AOSpeed => 1.05f;
		public override float AOSize => .9f;
		public override float AODamage => 1.05f;
		public override int AOValue => 40;
		public override AOItemTiers AOWeaponTier => AOItemTiers.Average;
		public override AORarities AORarity => AORarities.Uncommon;

		public override WeaponAbility? Ability => new(this, Color.Orange);

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.height = Item.width = 46;
			Item.useTurn = true;
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.DamageType = TrueMelee();
			Item.shoot = ModContent.ProjectileType<BronzeRapierProjectile>();
			Item.shootSpeed = 1f;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BronzeBar>(8).AddIngredient<OldRapier>().AddTile(TileID.Anvils).Register();
		}

		private bool canSwing = true;
		public override bool CanUseItem(Player player)
		{
			canSwing = !canSwing;
			if (!canSwing && !player.AltUse())
			{
				if (Item.useStyle == ItemUseStyleID.Thrust)
					Item.useStyle = ItemUseStyleID.Swing;
				else
					Item.useStyle = ItemUseStyleID.Thrust;
				Item.noMelee = false;
				Item.noUseGraphic = false;
				return canSwing;
			}
			if (player.AltUse())
			{
				canSwing = true;
				Item.useStyle = ItemUseStyleID.Rapier;
				Item.noMelee = true;
				Item.noUseGraphic = true;
				return true;
			}
			return base.CanUseItem(player);
		}

		public override bool CanShoot(Player player)
		{
			return player.AltUse() && player.ownedProjectileCounts[Item.shoot] < 1;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			var shot = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
			var dash = new PiercingStrikes(Item, shot);
			player.ArcaneOdyssey().StartDash(dash, imbue: Imbue, imbueAffectsSpeed: true);
			return false;
		}

		public override bool AltFunctionUse(Player player) => !player.ArcaneOdyssey().OnCooldown<PiercingStrikesCooldown>();
	}
}
