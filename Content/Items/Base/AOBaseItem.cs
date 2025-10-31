using ArcaneOdyssey.VFX.Rarities;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Base
{
	public abstract class AOBaseItem : ModItem
	{
		public virtual AORarities AORarity => AORarities.RESOLVESELF;
		public virtual ItemType ItemType => ItemType.RESOLVESELF;

		public override void SetDefaults()
		{
			if (AORarity != AORarities.RESOLVESELF && AORarity != AORarities.Special)
				Item.rare = (int)AORarity;
			if (AORarity == AORarities.Special) 
			{
				Item.rare = ModContent.RarityType<HotPinkRare>();
			}
		}
	}
}
