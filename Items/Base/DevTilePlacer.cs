namespace ArcaneOdyssey.Items.Base
{
	public abstract class DevTilePlacer : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Special;
		public override bool IsLoadingEnabled(Mod mod) => ArcaneOdysseyMod.DevMode;
		public abstract int Tile { get; }

		public override string Texture => AOUtils.GelTexture;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DefaultToPlaceableTile(Tile);
			Item.tileBoost = 100;
		}
	}
}
