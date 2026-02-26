using ArcaneOdyssey.Content.Items.Base;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Items.Weapons
{
	public class Sinbane : AORangedOrMeleeWeapon
	{
		public override int AOValue => 400;
		public override AORarities AORarity => AORarities.Uncommon;
		public override AOItemTiers AOWeaponTier => AOItemTiers.Good;
		public override float AOSpeed => 1.1f;
		public override float AOSize => .8f;
		public override float AODamage => 1.1f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useStyle = ItemUseStyleID.Swing;
			Item.DamageType = AOUtils.TrueMelee();
			Item.width = Item.height = 76;
		}
	}
}
