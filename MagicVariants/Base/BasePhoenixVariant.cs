using ArcaneOdyssey.Imbues.Magic.Lost;

namespace ArcaneOdyssey.MagicVariants.Base
{
	public abstract class BasePhoenixVariant : MagicVariant<PhoenixMagic>
	{
		public string WingsTexture => Texture + "Wings";
		public Asset<Texture2D> Wings = null;
		public override void Load()
		{
			ModContent.RequestIfExists(WingsTexture, out Wings);
		}
	}
}
