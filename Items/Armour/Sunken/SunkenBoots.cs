using ArcaneOdyssey.Items.Materials;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Armour.Sunken
{
	[AutoloadEquip(EquipType.Legs)]
	public class SunkenBoots : Base.Armour
	{
		public override AOItemTiers ArmourTier => AOItemTiers.Good;
		public override int AODefense => 145;
		public override int Size => AOAttkSpd;
		public override int AOAttkSpd => 16;
		public override AORarities AORarity => AORarities.Rare;

		public override int AOValue => 675;

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<SunkenScrap>(3).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
