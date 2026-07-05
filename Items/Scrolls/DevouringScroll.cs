using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.EmptyScrolls;
using Terraria;

namespace ArcaneOdyssey.Items.Scrolls
{
	public class DevouringScroll : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Common;
		public override string Texture => AOUtils.GetTexture<EmptyScroll>();

		public override void RightClick(Player player)
		{
			if (player.PlayerItem()?.ModItem is Imbuable imbue)
			{
				for (byte i = 0; i < imbue.Skills.Length; i++)
				{
					imbue.RemoveSkill(i);
				}
			}
		}

		public override bool CanRightClick() => Main.LocalPlayer.PlayerItem()?.ModItem is Imbuable;
	}
}
