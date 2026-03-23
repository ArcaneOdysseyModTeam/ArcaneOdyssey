using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;


namespace ArcaneOdyssey.Items.Weapons.Old
{
	public class WoodenStaff : Weapon
	{
		public override float AOSpeed => 1.05f;
		public override float AOSize => 0.9f;
		public override float AODamage => 1f;
		public override int AOValue => 1350;
		public override AORarities AORarity => AORarities.Common;
		public override AOItemTiers AOWeaponTier => AOItemTiers.Poor;
		public override Debuff? WeaponDebuff => null; // dull weapon
		public override SoundStyle UseSound => SoundID.Item1;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<OldGreataxe>();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DamageType = AOUtils.TrueMeleeNoSpeed();
			Item.shoot = ModContent.ProjectileType<WoodenStaffProjectile>();
			Item.width = Item.height = 60;
			Item.channel = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.reuseDelay = 120;
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}

		public override Color Motif => Color.Brown;
	}
}
