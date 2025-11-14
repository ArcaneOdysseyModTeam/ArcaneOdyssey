using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Armour.Centurion
{
    [AutoloadEquip(EquipType.Body)]
    public class RavennaChest : AOArmour
    {
        public override AOItemTiers ArmourTier => AOItemTiers.Average;
        public override int AODefense => 277;
        public override int AOSize => AODefense / 20;
        public override int AOAttkSpd => AODefense / 20;
        public override AORarities AORarity => AORarities.Uncommon;

        public override int AOValue => 167;

		public override void SetDefaults()
		{
			base.SetDefaults();
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<BronzeBar>(20).AddTile(TileID.Anvils).Register();
        }
    }
}
