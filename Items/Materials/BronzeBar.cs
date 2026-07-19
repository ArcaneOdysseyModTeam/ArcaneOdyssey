using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Tiles.Bronze;

namespace ArcaneOdyssey.Items.Materials
{
	public class BronzeBar : BaseItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Item.ResearchUnlockCount = 25;
		}

		public override ItemRarities Rarity => ItemRarities.Uncommon;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = 30;
			Item.height = 24;
			Item.createTile = ModContent.TileType<BronzeBarTile>();
			Item.maxStack = Item.CommonMaxStack;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
		}

		public override int Value => 30;

		public override void AddRecipes()
		{
			RecipeGroup coppergroup = new(() => Language.GetTextValue("LegacyMisc.37") + Lang.GetItemNameValue(ItemID.CopperOre), ItemID.CopperOre, ItemID.TinOre);
			var cop = RecipeGroup.RegisterGroup("AnyCopperOre", coppergroup);
			RecipeGroup goldgroup = new(() => Language.GetTextValue("LegacyMisc.37") + Lang.GetItemNameValue(ItemID.GoldOre), ItemID.GoldOre, ItemID.PlatinumOre);
			var gold = RecipeGroup.RegisterGroup("AnyGoldOre", goldgroup);
			RecipeGroup evilgroup = new(() => Language.GetTextValue("LegacyMisc.37") + Lang.GetItemNameValue(ItemID.ShadowScale), ItemID.ShadowScale, ItemID.TissueSample);
			var evil = RecipeGroup.RegisterGroup("AnyShadowScale", evilgroup);
			CreateRecipe(10).AddRecipeGroup(cop, 4).AddRecipeGroup(gold, 4).
				AddRecipeGroup(evil).AddTile(TileID.Hellforge).Register();
		}
	}
}