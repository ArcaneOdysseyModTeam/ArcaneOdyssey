using ArcaneOdyssey.Content.Items.Base;
using Terraria.ID;


namespace ArcaneOdyssey.Content.Items.Weapons.Old
{
	public class OldSword : AORangedOrMeleeWeapon
	{
		public override int AOValue => 40;
		public override float AOSize => 1;
		public override float AOSpeed => 1.05f;
		public override float AODamage => .9f;
		public override AORarities AORarity => AORarities.Common;
		public override AOItemTiers AOWeaponTier => AOItemTiers.Poor;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.height = 42;
			Item.DamageType = AOUtils.TrueMelee();
			Item.height = 42;
			Item.useTurn = true;
			Item.useStyle = ItemUseStyleID.Thrust;
		}
	}
}
