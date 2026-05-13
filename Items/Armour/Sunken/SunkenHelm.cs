using ArcaneOdyssey.Items.Materials;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Armour.Sunken
{
	[AutoloadEquip(EquipType.Head)]
	public class SunkenHelm : Base.Armour
	{
		public override ItemTiers ArmourTier => ItemTiers.Good;
		public override ushort AODefense => 204;
		public override short Size => (short)AOAttkSpd;
		public override short AOAttkSpd => 16;
		public override ItemRarities Rarity => ItemRarities.Rare;
		public override int Value => 675;

		public override short AOPower => 7;
		public override byte MaxMana => 40;

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<SunkenScrap>(4).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
