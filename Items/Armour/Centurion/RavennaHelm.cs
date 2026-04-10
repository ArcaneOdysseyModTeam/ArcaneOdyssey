using ArcaneOdyssey.Items.Materials;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Armour.Centurion
{
	[AutoloadEquip(EquipType.Head)]
	public class RavennaHelm : Base.Armour
	{
		public override ItemTiers ArmourTier => ItemTiers.Average;
		public override int AODefense => 144;
		public override int Size => AODefense / 17;
		public override int AOAttkSpd => AODefense / 17;
		public override Rarities Rarity => Rarities.Uncommon;
		public override int Value => 55;

		public override void SetDefaults()
		{
			base.SetDefaults();
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BronzeBar>(10).AddTile(TileID.Anvils).Register();
		}
	}
}
