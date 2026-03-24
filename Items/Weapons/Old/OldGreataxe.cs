using ArcaneOdyssey.Items.Base;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;


namespace ArcaneOdyssey.Items.Weapons.Old
{
	public class OldGreataxe : Weapon
	{
		public override int AOValue => 50;
		public override float AOSize => 1.05f;
		public override float AOSpeed => .9f;
		public override float AODamage => 1;
		public override Rarities Rarity => Rarities.Common;
		public override AOItemTiers AOWeaponTier => AOItemTiers.Poor;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<OldGreatsword>();
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
