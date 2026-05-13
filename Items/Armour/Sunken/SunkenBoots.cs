using ArcaneOdyssey.Items.Materials;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Armour.Sunken
{
	[AutoloadEquip(EquipType.Legs)]
	public class SunkenBoots : Base.Armour
	{
		public override ItemTiers ArmourTier => ItemTiers.Good;
		public override ushort AODefense => 145;
		public override short Size => (short)AOAttkSpd;
		public override short AOAttkSpd => 16;
		public override ItemRarities Rarity => ItemRarities.Rare;

		public override int Value => 675;

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<SunkenScrap>(3).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
