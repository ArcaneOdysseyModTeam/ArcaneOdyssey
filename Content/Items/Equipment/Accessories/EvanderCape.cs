using ArcaneOdyssey.Content.Items.Base;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Equipment.Accessories
{
	public class EvanderCape : AOArmour
	{
		public override AOItemTiers ArmourTier => AOItemTiers.Good;
		public override int AODefense => 170;
		public override int AOPierce => 21;
		public override int AOValue => 75;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
		}
	}
}
