using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Abilities;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Items.Weapons.Atlantean
{
	public class AtlanteanClaws : Weapon
	{
		public override ItemTiers WeaponTier => ItemTiers.Great;

		public override Color Motif => Color.PaleVioletRed;

		public override ItemRarities Rarity => ItemRarities.Uncommon;

		public override float Size => 1.2f;

		public override float Speed => 1.05f;

		public override float Damage => .925f;

		public override int Value => 20;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArcaneOdysseyMod.Sets.atlanteanItem[Type] = true;
			Item.claw[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.height = Item.height = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shoot = ModContent.ProjectileType<BeastInstinct>();
			Item.DamageType = AOUtils.TrueMelee();
			Item.shootSpeed = 1f;
		}

		public override bool CanShoot(Player player) => player.AltUse() && player.ownedProjectileCounts[Item.shoot] < 1;

		public override bool AltFunctionUse(Player player) => !(player.ArcaneOdyssey().HeavySkillActive || player.ArcaneOdyssey().OnCooldown<BeastInstinctCooldown>());

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			ActivateAbility(player, false);
			player.ArcaneOdyssey().SetCooldown<BeastInstinctCooldown>();
			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}
	}

	public class BeastInstinctCooldown : DisplayedCooldown
	{
		public override string Texture => AOUtils.GetTexture<AtlanteanClaws>();
		public override int CooldownLength => 180;
	}
}
