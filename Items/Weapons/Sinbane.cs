using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Abilities;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Items.Weapons
{
	public class Sinbane : Weapon
	{
		public override int Value => 400;
		public override ItemRarities Rarity => ItemRarities.Uncommon;
		public override ItemTiers WeaponTier => ItemTiers.Good;
		public override float Speed => 1.1f;
		public override float Size => .8f;
		public override float Damage => 1.1f;

		public override Color Motif => new(104, 130, 0);

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArcaneOdysseyMod.Sets.greatsword[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useStyle = ItemUseStyleID.Swing;
			Item.DamageType = DamageClass.Melee;
			Item.width = Item.height = 60;
			Item.shoot = ModContent.ProjectileType<ToweringImpact>();
			Item.shootSpeed = 3f;
		}

		public override bool AltFunctionUse(Player player) => !player.ArcaneOdyssey().OnCooldown<ToweringImpactCooldown>();

		public override bool CanShoot(Player player) => player.AltUse();

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			ActivateAbility(player, false);
			player.ArcaneOdyssey().SetCooldown<ToweringImpactCooldown>();
			return true;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			knockback *= 3f;
		}


	}

	public class ToweringImpactCooldown : DisplayedCooldown
	{
		public override string Texture => AOUtils.GetTexture<Sinbane>();

		public override int CooldownLength => 60 * 5;
	}
}
