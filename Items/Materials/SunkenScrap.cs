using ArcaneOdyssey.Items.Base;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Materials
{
	[LegacyName("ArcaniumScrap")]
	public class SunkenScrap : BaseItem
	{
		public int AOValue = 400;
		public override AORarities AORarity => AORarities.Rare;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 28;
			Item.maxStack = Item.CommonMaxStack;
			Item.value = AOUtils.GalleonToCopper(AOValue);
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Item.ResearchUnlockCount = 25;
		}
	}
}
