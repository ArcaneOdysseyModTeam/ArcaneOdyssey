using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Items.Weapons.Old;
using ArcaneOdyssey.Content.Projectiles.Weapons.Abilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
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
			CreateRecipe().AddIngredient<BronzeBar>(12).AddIngredient<OldSword>().AddTile(TileID.Anvils).Register();
		}

		public override bool AltFunctionUse(Player player)
		{
			if (!player.ArcaneOdyssey().OnCooldown(nameof(WhirlwindCooldown)))
			{
				player.ArcaneOdyssey().SetCooldown(new WhirlwindCooldown().AOCooldown);
				var proj = Projectile.NewProjectileDirect(Item.GetSource_FromThis(), player.Center, Vector2.UnitX * player.direction, ModContent.ProjectileType<Whirlwind>(), Item.damage, 0, player.whoAmI);
				((Whirlwind)proj.ModProjectile).color = this.Imbue() is not null ? Color.Lerp(Color.Orange, this.Imbue().GetColor(), .5f) : Color.Orange;
				SoundEngine.PlaySound(Item.UseSound, player.Center);
			}
			return false;
		}
	}
}
