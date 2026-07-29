using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Abilities;

namespace ArcaneOdyssey.Items.Weapons.RavennaNoble
{
	public class ScimitarofStorm : Weapon
	{
		public override int Value => 210;

		public override ItemTiers WeaponTier => ItemTiers.Average;

		public override ItemRarities Rarity => ItemRarities.Uncommon;

		public override float Speed => 1.15f;
		public override float Damage => 1.05f;
		public override float Size => .85f;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArcaneOdysseyMod.Sets.dualbladed[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useTime = (Item.useAnimation / 2) + 1;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.DamageType = DamageClass.Melee;
			Item.shoot = ModContent.ProjectileType<TwinCrescent>();
			Item.shootSpeed = 7f;
		}

		public override bool CanShoot(Player player) => usingWithAbility;

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			damage /= 2;
			knockback /= 2;
		}

		public override Color Motif => Color.MediumPurple;

		private bool usingWithAbility = false;

		public override void UseAnimation(Player player)
		{
			if (!player.OnCooldown<TwinCrecsentsCooldown>())
			{
				ActivateAbility(player, true);
				usingWithAbility = true;
				player.SetCooldown<TwinCrecsentsCooldown>();
			}
			else
			{
				usingWithAbility = false;
			}	
		}
	}

	public class TwinCrecsentsCooldown : DisplayedCooldown
	{
		public override string Texture => AOUtils.GetTexture<ScimitarofStorm>();

		public override int CooldownLength => 75;
	}
}
