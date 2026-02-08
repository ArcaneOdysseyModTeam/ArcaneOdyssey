using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Armour.Sunken
{
	[AutoloadEquip(EquipType.Body)]
	public class SunkenChest : AOArmour
	{
		public override AOItemTiers ArmourTier => AOItemTiers.Good;
		public override int AODefense => 194;
		public override int AOSize => AOAttkSpd;
		public override int AOAttkSpd => 22;
		public override AORarities AORarity => AORarities.Rare;

		public override int AOValue => 1350;

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<SunkenScrap>(5).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
