using ArcaneOdyssey.Imbues.Relics;

namespace ArcaneOdyssey.Imbues.Enemies
{
	public class LaelusBand : TidestoneBand
	{
		public override string Texture => AOUtils.GetTexture<TidestoneBand>();
		protected override Color? SpiritColourOverride => GoodColour;
	}
}
