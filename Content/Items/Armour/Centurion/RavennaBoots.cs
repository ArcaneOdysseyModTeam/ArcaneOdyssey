using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Armour.Centurion
{
	[AutoloadEquip(EquipType.Legs)]
	public class RavennaBoots : AOArmour
	{
		public override AOItemTiers ArmourTier => AOItemTiers.Poor;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			if (Main.netMode != NetmodeID.Server)
			{
				EquipLoader.GetEquipSlot(Mod, Name, EquipType.Shield);
			}
		}

		public override void Load()
		{
			if (Main.netMode != NetmodeID.Server)
			{
				EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Shield}", EquipType.Shield, this);
			}
		}

		public override int AODefense => 56;
		public override int AOSize => AODefense / 20;
		public override int AOAttkSpd => AODefense / 20;
		public override AORarities AORarity => AORarities.Common;
		public override int AOValue => 30;

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BronzeBar>(15).AddTile(TileID.Anvils).Register();
		}
	}
}
