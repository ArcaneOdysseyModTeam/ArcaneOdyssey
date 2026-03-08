using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Items.Weapons.Old;
using ArcaneOdyssey.Content.Projectiles.Abilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;


namespace ArcaneOdyssey.Content.Items.Weapons.Bronze
{
	public class RavennaSword : AOWeapon
	{
		public override int AOValue => 50;
		public override float AOSize => 1;
		public override float AOSpeed => .95f;
		public override float AODamage => 1.05f;
		public override AORarities AORarity => AORarities.Uncommon;
		public override AOItemTiers AOWeaponTier => AOItemTiers.Average;
		public override Color Colour => Color.Orange;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.height = 40;
			Item.height = 40;
			Item.useTurn = true;
			Item.DamageType = AOUtils.TrueMelee();
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
