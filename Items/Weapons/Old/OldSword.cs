using ArcaneOdyssey.Items.Base;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;


namespace ArcaneOdyssey.Items.Weapons.Old
{
	public class OldSword : AOWeapon
	{
		public override int AOValue => 40;
		public override float AOSize => 1;
		public override float AOSpeed => 1.05f;
		public override float AODamage => .9f;
		public override AORarities AORarity => AORarities.Common;
		public override AOItemTiers AOWeaponTier => AOItemTiers.Poor;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<WoodenStaff>();
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
