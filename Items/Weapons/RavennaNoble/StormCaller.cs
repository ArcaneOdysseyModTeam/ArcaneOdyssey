using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Abilities;
using Terraria.Audio;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Items.Weapons.RavennaNoble
{
	public class StormCaller : Weapon
	{
		public override float Damage => 0.9f;
		public override float Size => 1.1f;
		public override float Speed => 1.15f;
		public override int Value => 120;
		public override ItemTiers WeaponTier => ItemTiers.Average;
		public override ItemRarities Rarity => ItemRarities.Uncommon;
		public override SoundStyle UseSound => SoundID.Item5;
		public override Color Motif => Color.MediumPurple;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.noMelee = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 18;
			Item.height = 56;
			Item.autoReuse = true;
			Item.shoot = ProjectileID.WoodenArrowFriendly;
			Item.shootSpeed = 12f;
			Item.useAmmo = AmmoID.Arrow;
		}

		public override bool AltFunctionUse(Player player) => !player.ArcaneOdyssey().OnCooldown<StormofArrowsCooldown>();

		public override Vector2? HoldoutOffset() => new();

		public override void OnConsumeAmmo(Item ammo, Player player)
		{
			if (player.AltUse())
			{
				for (int i = 0; i < 4; i++)
				{
					player.ConsumeItem(ammo.type);
				}
			}
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (player.AltUse())
			{
				ActivateAbility(player, true);
				var offsetX = Main.MouseWorld.X;
				var offsetY = Main.screenPosition.Y;
				var pos = new Vector2(offsetX, offsetY);
				velocity = player.SafeDirectionTo(pos) * velocity.Length();
				player.ArcaneOdyssey().SetCooldown<StormofArrowsCooldown>();
			}
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.AltUse())
			{
				Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<ArrowStorm>(), damage, knockback, player.whoAmI, type);
				return false;
			}
			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}
	}

	public class StormofArrowsCooldown : DisplayedCooldown
	{
		public override string Texture => AOUtils.GetTexture<StormCaller>();

		public override int CooldownLength => 120;
	}
}
