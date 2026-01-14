using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Items.Weapons.Old;
using ArcaneOdyssey.Content.Projectiles.Weapons.Abilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Weapons.Bronze
{
	public class RavennaSword : AORangedOrMeleeWeapon
	{
		public override int AOValue => 50;
		public override float AOSize => 1;
		public override float AOSpeed => .95f;
		public override float AODamage => 1.05f;
		public override AORarities AORarity => AORarities.Uncommon;
		public override AOItemTiers AOWeaponTier => AOItemTiers.Average;
		public override WeaponAbility? Ability => new(Mod, "Whirlwind", "Spin your weapon around quickly, dealing damage to surrounding enemies and holding yourself in place", Color.Orange);

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.height = 40;
			Item.height = 40;
			Item.useTurn = true;
			Item.DamageType = TrueMelee();
			Item.useStyle = ItemUseStyleID.Thrust;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BronzeBar>(8).AddIngredient<OldSword>().AddTile(TileID.Anvils).Register();
		}

		public override bool AltFunctionUse(Player player)
		{
			if (player.ownedProjectileCounts[Item.shoot] < 1 && !player.ArcaneOdyssey().OnCooldown(ModContent.BuffType<WhirlwindCooldown>()))
			{
				player.ArcaneOdyssey().SetCooldown(new WhirlwindCooldown());
				var proj = Projectile.NewProjectileDirect(new EntitySource_ItemUse(player, Item), player.Center, Vector2.UnitX * player.direction, ModContent.ProjectileType<Whirlwind>(), Item.damage, 0, player.whoAmI);
				((Whirlwind)proj.ModProjectile).colour = proj.Imbue()?.GetColor(Color.Orange) ?? Color.Orange;
				SoundEngine.PlaySound(Item.UseSound, player.Center);
			}
			return true;
		}

		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			if (player.ArcaneOdyssey().WhirlwindActive)
				damage *= 0;
		}
	}
}
