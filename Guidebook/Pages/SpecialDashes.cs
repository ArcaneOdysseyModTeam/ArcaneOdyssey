using Terraria;

namespace ArcaneOdyssey.Guidebook.Pages
{
	public class SpecialDashes : GuidebookPage
	{
		public override ushort PageNum => After<UsingScrolls>();

		public override bool MetConditions(Player player)
		{
			if (player.ArcaneOdyssey()?.OmniDash is not null)
			{
				return true;
			}
			return false;
		}
	}
}
