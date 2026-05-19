using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Materials;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Armour.Centurion
{
	[AutoloadEquip(EquipType.Head)]
	public class RavennaHelm : BaseArmour
	{
		public override ItemTiers ArmourTier => ItemTiers.Average;
		public override ushort AODefense => 144;
		public override short Size => (short)(AODefense / 17);
		public override short AOAttkSpd => (short)(AODefense / 17);
		public override ItemRarities Rarity => ItemRarities.Uncommon;
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
