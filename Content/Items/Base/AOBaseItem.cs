using ArcaneOdyssey.VFX.Rarities;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Base
{
	public abstract class AOBaseItem : ModItem
	{
		public abstract AORarities AORarity { get; }

		public virtual ItemType? ItemCategory => null;

		public virtual bool ShowItemTypeTooltip => true;

		public override void SetDefaults()
		{
			if (AORarity != AORarities.Special)
				Item.rare = (int)AORarity;
			if (AORarity == AORarities.Special)
			{
				Item.rare = ModContent.RarityType<HotPinkRare>();
			}
		}
	}
}
