using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Accessories.Helpers;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Skills.Base;

namespace ArcaneOdyssey.Items.Scrolls.Mobility.Rare
{
	public class FlightScroll : RareScroll
	{
		public override bool CanHaveMagic => true;
		public override bool CanHaveRelic => true;
		public override ModSkill Skill => ModContent.GetInstance<BasicFlight>();
	}

	public class BasicFlight : ModSkill
	{
		public override SkillType SkillSlot => SkillType.Mobility;

		public override int Scroll => ModContent.ItemType<FlightScroll>();

		public override void Activate(Player player, Imbuable imbue)
		{
			player.ApplyEquipFunctional(new Item(ModContent.ItemType<FlightCore>()), false);
		}
	}
}
