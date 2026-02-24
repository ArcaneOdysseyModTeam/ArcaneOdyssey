using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Weapons.RavennaNoble
{
	public class StormCaller : AORangedOrMeleeWeapon
	{
		public override float AODamage => 0.9f;
		public override float AOSize => 1.1f;
		public override float AOSpeed => 1.15f;
		public override int AOValue => 120;
		public override AOItemTiers AOWeaponTier => AOItemTiers.Average;
		public override AORarities AORarity => AORarities.Rare;
		public override SoundStyle UseSound => SoundID.Item5;
		public override WeaponAbility? Ability => new(this, Color.MediumPurple);

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

		public override bool AltFunctionUse(Player player) => true;

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

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.AltUse())
			{
				var offsetX = Main.MouseWorld.X + (Main.screenWidth / 35f * Main.rand.Next(-2, 3));
				var offsetY = Main.screenPosition.Y - (Main.screenHeight * .15f);
				var pos = new Vector2(offsetX, offsetY);
				player.itemRotation = player.MountedCenter.DirectionTo(pos).ToRotation();
				if (player.direction != 1)
				{
					player.itemRotation += MathHelper.Pi;
				}
				for (int i = -2; i < 3; i++)
				{
					offsetX = Main.MouseWorld.X + (Main.screenWidth / 35f * i);
					offsetY = Main.screenPosition.Y - (Main.screenHeight * .15f);
					pos = new Vector2(offsetX, offsetY);
					Projectile.NewProjectile(source, pos, Vector2.UnitY * velocity.Length(), type, damage / 5, knockback / 5f, player.whoAmI);
				}
				return false;
			}
			return true;
		}
	}
}
