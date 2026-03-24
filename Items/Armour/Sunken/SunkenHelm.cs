using ArcaneOdyssey.Items.Materials;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Armour.Sunken
{
	[AutoloadEquip(EquipType.Head)]
	public class SunkenHelm : Base.Armour
	{
		public override AOItemTiers ArmourTier => AOItemTiers.Good;
		public override int AODefense => 204;
		public override int Size => AOAttkSpd;
		public override int AOAttkSpd => 16;
		public override AORarities AORarity => AORarities.Rare;
		public override int AOValue => 675;

		public override int AOPower => 7;
		public override int AOMaxMana => 40;

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<SunkenScrap>(4).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
