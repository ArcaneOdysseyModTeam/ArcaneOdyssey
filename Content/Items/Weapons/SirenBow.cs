using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Weapons.RavennaNoble;
using ArcaneOdyssey.Content.Projectiles.Weapons.Abilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Weapons
{
	public class SirenBow : AOWeapon
	{
		public override float AODamage => 1.025f;
		public override float AOSize => .825f;
		public override float AOSpeed => 1.15f;
		public override int AOValue => 100;
		public override AOItemTiers AOWeaponTier => AOItemTiers.Great;
		public override AORarities AORarity => AORarities.Uncommon;
		public override SoundStyle UseSound => SoundID.Item5;
		public override Color Colour => Color.Gold;

		public override string Texture => AOUtils.GetTexture<StormCaller>();

		public static float AttacksPerUse => 2f;

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
			Item.shootSpeed = 15f;
			Item.useAmmo = AmmoID.Arrow;
		}

		public override float UseTimeMultiplier(Player player)
		{
			if (player.AltUse())
				return 1f / AttacksPerUse;
			return 1f;
		}

		public override bool AltFunctionUse(Player player) => true;

		public override Vector2? HoldoutOffset() => new();

		public override void OnConsumeAmmo(Item ammo, Player player)
		{
			if (player.AltUse())
			{
				for (int i = 0; i <= AttacksPerUse; i++)
				{
					player.ConsumeItem(ammo.type);
				}
			}
		}

		public override void UseAnimation(Player player)
		{
			if (player.AltUse())
				ActivateAbility(player, true);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (player.AltUse())
			{
				var offsetX = Main.MouseWorld.X;
				var offsetY = Main.screenPosition.Y;
				var pos = new Vector2(offsetX, offsetY);
				pos += new Vector2(Main.rand.NextFloat(ApplyImbueSpeed(-5f * 16, true), ApplyImbueSpeed(5f * 16, true)), Main.rand.NextFloat(ApplyImbueSpeed(-5f * 16, true), ApplyImbueSpeed(5f * 16, true)));
				velocity = player.MountedCenter.DirectionTo(pos) * velocity.Length();
				damage = (damage / AttacksPerUse).Round();
			}
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.AltUse())
			{
				Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<ArrowRain>(), damage, knockback, player.whoAmI, type);
				return false;
			}
			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}
	}
}
