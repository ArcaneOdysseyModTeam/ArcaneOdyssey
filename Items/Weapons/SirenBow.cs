using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Abilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Weapons
{
	public class SirenBow : Weapon
	{
		public override float Damage => 1.025f;
		public override float Size => .825f;
		public override float Speed => 1.15f;
		public override int Value => 100;
		public override ItemTiers WeaponTier => ItemTiers.Great;
		public override Rarities Rarity => Rarities.Uncommon;
		public override SoundStyle UseSound => SoundID.Item5;
		public override Color Motif => Color.Gold;

		public static float AttacksPerUse => 2.5f;

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
			Item.width = 16;
			Item.height = 46;
			Item.autoReuse = true;
			Item.shoot = ProjectileID.WoodenArrowFriendly;
			Item.shootSpeed = 15f;
			Item.useAmmo = AmmoID.Arrow;
			Item.scale *= 1.5f;
		}

		public override float UseTimeMultiplier(Player player)
		{
			if (player.AltUse())
				return 1f / AttacksPerUse;
			return 1f;
		}

		public override bool AltFunctionUse(Player player) => true;

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

		 //unobtainable

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
				velocity = player.SafeDirectionTo(pos) * velocity.Length();
			}
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.AltUse())
			{
				var offsetX = Main.MouseWorld.X;
				var offsetY = Main.screenPosition.Y;
				var pos = new Vector2(offsetX, offsetY);
				pos += new Vector2(Main.rand.NextFloat(ApplySpeed(-5f * 16, true), ApplySpeed(5f * 16, true)), Main.rand.NextFloat(ApplySpeed(-5f * 16, true), ApplySpeed(5f * 16, true)));
				velocity = player.SafeDirectionTo(pos) * velocity.Length();
				Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<ArrowRain>(), damage, knockback, player.whoAmI, type);
				return false;
			}
			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}
	}
}
