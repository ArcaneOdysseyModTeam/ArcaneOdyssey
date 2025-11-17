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
		public override int AODefense => 204;
		public override int AOSize => 23;
		public override int AOAttkSpd => 23;
		public override AORarities AORarity => AORarities.Rare;

		public override int AOValue => 2700;

		public override int MinionSlots => 2;

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<SunkenScrap>(5).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
