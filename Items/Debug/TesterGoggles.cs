using ArcaneOdyssey.Items.Base;

namespace ArcaneOdyssey.Items.Debug
{
	public class TesterGoggles : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Special;
		public override string Texture => AOUtils.TerrariaItemTexture(ItemID.MechanicalLens);
	}
}
