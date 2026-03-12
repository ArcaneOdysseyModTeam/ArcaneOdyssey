using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Walls.UnsafeBronze;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Blocks.Walls.UnsafeBronze
{
	public class UnsafeBronzeColumnWallItem : AOBaseItem
	{
		public override AORarities AORarity => AORarities.Common;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 400;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DefaultToPlaceableWall(ModContent.WallType<UnsafeBronzeColumnWall>());
		}
	}
}
