using ArcaneOdyssey.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Armour.Centurion
{
	[AutoloadEquip(EquipType.Legs)]
	public class RavennaBoots : Base.Armour
	{
		public override ItemTiers ArmourTier => ItemTiers.Poor;

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

		public override ushort AODefense => 56;
		public override short Size => (short)(AODefense / 20);
		public override short AOAttkSpd => (short)(AODefense / 20);
		public override ItemRarities Rarity => ItemRarities.Common;
		public override int Value => 30;

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BronzeBar>(15).AddTile(TileID.Anvils).Register();
		}
	}
}
