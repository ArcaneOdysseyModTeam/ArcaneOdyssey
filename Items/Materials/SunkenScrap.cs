using ArcaneOdyssey.Items.Base;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Materials
{
	[LegacyName("ArcaniumScrap")]
	public class SunkenScrap : BaseItem
	{
		public override int AOValue => 400;
		public override Rarities Rarity => Rarities.Rare;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 28;
			Item.maxStack = Item.CommonMaxStack;
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Item.ResearchUnlockCount = 25;
		}
	}
}
