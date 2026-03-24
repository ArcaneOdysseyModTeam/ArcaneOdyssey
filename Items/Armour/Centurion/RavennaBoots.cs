using ArcaneOdyssey.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Armour.Centurion
{
	[AutoloadEquip(EquipType.Legs)]
	public class RavennaBoots : Base.Armour
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
		public override int Size => AODefense / 20;
		public override int AOAttkSpd => AODefense / 20;
		public override Rarities Rarity => Rarities.Common;
		public override int AOValue => 30;

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BronzeBar>(15).AddTile(TileID.Anvils).Register();
		}
	}
}
