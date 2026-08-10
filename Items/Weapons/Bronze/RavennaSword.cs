using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Materials;
using ArcaneOdyssey.Items.Weapons.Old;
using ArcaneOdyssey.Projectiles.Abilities;
using Terraria.Audio;
using Terraria.DataStructures;


namespace ArcaneOdyssey.Items.Weapons.Bronze
{
	public class RavennaSword : Weapon
	{
		public override int Value => 50;
		public override float Size => 1;
		public override float Speed => .95f;
		public override float Damage => 1.05f;
		public override ItemRarities Rarity => ItemRarities.Uncommon;
		public override ItemTiers WeaponTier => ItemTiers.Average;
		public override Color Motif => Color.Orange;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.height = 40;
			Item.height = 40;
			Item.useTurn = true;
			Item.DamageType = DamageClass.Melee;
			Item.useStyle = ItemUseStyleID.Thrust;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BronzeBar>(8).AddIngredient<OldSword>().AddTile(TileID.Anvils).Register();
		}

		public override bool AltFunctionUse(Player player)
		{
			if (player.ownedProjectileCounts[Item.shoot] < 1 && !player.ArcaneOdyssey().OnCooldown<WhirlwindCooldown>())
			{
				ActivateAbility(player, false);
				player.ArcaneOdyssey()?.SetCooldown<WhirlwindCooldown>();
				var proj = Projectile.NewProjectileDirect(new EntitySource_ItemUse(player, Item), player.Center, Vector2.UnitX * player.direction, ModContent.ProjectileType<Whirlwind>(), Item.damage, 0, player.whoAmI);
				SoundEngine.PlaySound(Item.UseSound, player.Center);
			}
			return true;
		}
	}
}
