using ArcaneOdyssey.Items.Base;


namespace ArcaneOdyssey.Items.Weapons.Old
{
	public class OldSword : Weapon
	{
		public override int Value => 40;
		public override float Size => 1;
		public override float Speed => 1.05f;
		public override float Damage => .9f;
		public override ItemRarities Rarity => ItemRarities.Common;
		public override ItemTiers WeaponTier => ItemTiers.Poor;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<WoodenStaff>();
			ArcaneOdysseyMod.Sets.OldWeapon[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.height = 42;
			Item.DamageType = AOUtils.TrueMelee();
			Item.height = 42;
			Item.useTurn = true;
			Item.useStyle = ItemUseStyleID.Thrust;
		}

		public override Color Motif => Color.Gray;
	}
}
