using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Weapons.Bronze;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Weapons
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
		public const int ArrowCount = 5;

        public override void SetStaticDefaults()
        {
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

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.AltUse())
            {
                player.itemRotation = player.MountedCenter.DirectionTo(new Vector2(Main.MouseWorld.X, Main.screenPosition.Y)).ToRotation();
                if (player.direction != 1)
                {
                    player.itemRotation += MathHelper.Pi;
                }
                for (int i = -2; i < ArrowCount - 2; i++)
				{
					var offsetX = Main.MouseWorld.X + (Main.screenWidth / 30f * i);
					var offsetY = Main.screenPosition.Y - (Main.screenHeight * .15f);
					var pos = new Vector2(offsetX, offsetY);
                    var proj = Projectile.NewProjectileDirect(source, pos, Vector2.UnitY * velocity.Length(), type, damage / ArrowCount, knockback / ArrowCount, player.whoAmI);
                    proj.Center = pos;
                }
				return false;
			}
			return true;
		}
	}
}
