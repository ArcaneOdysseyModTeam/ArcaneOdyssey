using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Accessories.Helpers;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Skills.Base;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Scrolls.Equipment.Rare
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
			if (player.HasTypeInInventory<FlightCore>(out var core) || player.ArcaneOdyssey().hasWings > 0)
			{
				if (core is not null)
				{
					core.Imbue = imbue;
					core.SecondImbue = imbue.Imbue;
				}
			}
			else
			{
				player.QuickSpawnItemDirect(imbue.Item.GetSource_FromThis(), ModContent.ItemType<FlightCore>());
				player.ArcaneOdyssey().hasWings++;
			}
		}
	}
}
