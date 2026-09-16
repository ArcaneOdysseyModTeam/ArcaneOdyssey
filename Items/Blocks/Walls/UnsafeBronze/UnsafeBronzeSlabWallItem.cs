using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Blocks.Walls.Bronze;
using ArcaneOdyssey.Walls.UnsafeBronze;
using Terraria.GameContent.Creative;

namespace ArcaneOdyssey.Items.Blocks.Walls.UnsafeBronze
{
	public class UnsafeBronzeSlabWallItem : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Common;
		public override string Texture => AOUtils.GetTexture<BronzeSlabWallItem>();

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 400;
			ItemID.Sets.DrawUnsafeIndicator[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DefaultToPlaceableWall(ModContent.WallType<UnsafeBronzeSlabWall>());
		}
	}
}
