using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Armour.Sunken
{
	[AutoloadEquip(EquipType.Legs)]
	public class SunkenBoots : AOArmour
	{
		public override AOItemTiers ArmourTier => AOItemTiers.Good;
		public override int AODefense => 145;
		public override int AOSize => AOAttkSpd;
		public override int AOAttkSpd => 16;
		public override AORarities AORarity => AORarities.Rare;

		public override int AOValue => 675;

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<SunkenScrap>(3).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
