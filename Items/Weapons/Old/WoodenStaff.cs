using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Weapons;
using Terraria.Audio;


namespace ArcaneOdyssey.Items.Weapons.Old
{
	public class WoodenStaff : Weapon
	{
		public override float Speed => 1.05f;
		public override float Size => 0.9f;
		public override float Damage => 1f;
		public override int Value => 1350;
		public override ItemRarities Rarity => ItemRarities.Common;
		public override ItemTiers WeaponTier => ItemTiers.Poor;
		public override Debuff? WeaponDebuff => null; // dull weapon
		public override SoundStyle UseSound => SoundID.Item1;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<OldGreataxe>();
			ArcaneOdysseyMod.Sets.staff[Type] = true;
			ArcaneOdysseyMod.Sets.OldWeapon[Type] = true;
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
