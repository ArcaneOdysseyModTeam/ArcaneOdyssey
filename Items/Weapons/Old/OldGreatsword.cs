using ArcaneOdyssey.Items.Base;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;


namespace ArcaneOdyssey.Items.Weapons.Old
{
	public class OldGreatsword : AOWeapon
	{
		public override int AOValue => 40;
		public override float AOSize => 1.1f;
		public override float AOSpeed => .9f;
		public override float AODamage => 1.05f;
		public override AORarities AORarity => AORarities.Common;
		public override AOItemTiers AOWeaponTier => AOItemTiers.Poor;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<OldRapier>();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.height = Item.height = 60;
			Item.DamageType = AOUtils.TrueMelee();
			Item.useStyle = ItemUseStyleID.Swing;
		}

		public override Color Colour => Color.Gray;
	}
}
