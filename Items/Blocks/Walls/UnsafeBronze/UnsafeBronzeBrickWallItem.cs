using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Blocks.Walls.Bronze;
using ArcaneOdyssey.Walls.UnsafeBronze;
using Terraria.GameContent.Creative;

namespace ArcaneOdyssey.Items.Blocks.Walls.UnsafeBronze
{
	public class UnsafeBronzeBrickWallItem : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Common;
		public override string Texture => AOUtils.GetTexture<BronzeBrickWallItem>();

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 400;
			ItemID.Sets.DrawUnsafeIndicator[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DefaultToPlaceableWall(ModContent.WallType<UnsafeBronzeBrickWall>());
		}
	}
}
