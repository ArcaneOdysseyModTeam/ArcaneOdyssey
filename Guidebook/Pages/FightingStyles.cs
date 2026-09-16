using ArcaneOdyssey.Imbues.Base;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class FightingStyles : GuidebookPage
	{
		public override bool MetConditions(Player player) => player.HasTypeInInventory<FightingStyle>();
		public override ushort PageNum => After<MagicTypes>();
	}
}
