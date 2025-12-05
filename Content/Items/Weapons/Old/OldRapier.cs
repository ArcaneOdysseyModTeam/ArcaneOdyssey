using ArcaneOdyssey.Content.Items.Base;
using Terraria.ID;
using Terraria;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Weapons.Old
{
	public class OldRapier : AORangedOrMeleeWeapon
	{
		public override int AOValue => 20;
		public override float AOSize => .9f;
		public override float AOSpeed => 1.025f;
		public override float AODamage => 1.025f;
		public override AORarities AORarity => AORarities.Common;
		public override AOItemTiers AOWeaponTier => AOItemTiers.Poor;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DamageType = TrueMelee();
			Item.height = Item.height = 46;
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.DamageType = TrueMelee();
			Item.useTurn = true;
		}

		private bool canSwing = true;
		public override bool CanUseItem(Player player)
		{
			canSwing = !canSwing;
			if (!canSwing)
			{
				if (Item.useStyle == ItemUseStyleID.Thrust)
					Item.useStyle = ItemUseStyleID.Swing;
				else
					Item.useStyle = ItemUseStyleID.Thrust;
			}
			return base.CanUseItem(player) && canSwing;
		}
	}
}
