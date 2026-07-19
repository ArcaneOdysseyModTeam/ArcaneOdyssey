using ArcaneOdyssey.Items.Base;


namespace ArcaneOdyssey.Items.Weapons.Old
{
	public class OldGreataxe : Weapon
	{
		public override int Value => 50;
		public override float Size => 1.05f;
		public override float Speed => .9f;
		public override float Damage => 1;
		public override ItemRarities Rarity => ItemRarities.Common;
		public override ItemTiers WeaponTier => ItemTiers.Poor;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<OldGreatsword>();
			ArcaneOdysseyMod.Sets.greataxe[Type] = true;
			ArcaneOdysseyMod.Sets.OldWeapon[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.height = Item.height = 70;
			Item.axe = 70 / 5;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.DamageType = AOUtils.TrueMelee();
			Item.autoReuse = true;
		}

		public override Color Motif => Color.Gray;
	}
}
