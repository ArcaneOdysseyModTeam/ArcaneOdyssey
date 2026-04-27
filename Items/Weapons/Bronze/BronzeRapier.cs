using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Materials;
using ArcaneOdyssey.Items.Weapons.Old;
using ArcaneOdyssey.Projectiles.Abilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;


namespace ArcaneOdyssey.Items.Weapons.Bronze
{
	public class BronzeRapier : Weapon
	{
		public override float Speed => 1.05f;
		public override float Size => .9f;
		public override float Damage => 1.05f;
		public override int Value => 40;
		public override ItemTiers WeaponTier => ItemTiers.Average;
		public override ItemRarities Rarity => ItemRarities.Uncommon;

		public override Color Motif => Color.Orange;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArcaneOdysseyMod.Sets.rapier[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.height = Item.width = 46;
			Item.useTurn = true;
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.DamageType = AOUtils.TrueMelee();
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

		public override bool CanShoot(Player player) => player.AltUse() && player.ownedProjectileCounts[Item.shoot] < 1;
		

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			ActivateAbility(player, false);
			var shot = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
			var dash = new PiercingStrikes(shot);
			player.ArcaneOdyssey().StartDash(dash, imbue: Imbue, imbueAffectsSpeed: true);
			return false;
		}

		public override bool AltFunctionUse(Player player) => !player.ArcaneOdyssey().OnCooldown<PiercingStrikesCooldown>();
	}
}
