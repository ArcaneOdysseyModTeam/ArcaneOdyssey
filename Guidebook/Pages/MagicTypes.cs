using ArcaneOdyssey.Imbues.Base;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class MagicTypes : GuidebookPage
	{
		public override bool MetConditions(Player player) => player.HasTypeInInventory<MagicType>();
		public override ushort PageNum => After<GettingStarted>();
	}
}
