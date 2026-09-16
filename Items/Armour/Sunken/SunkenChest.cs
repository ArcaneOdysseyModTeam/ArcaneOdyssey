using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Materials;

namespace ArcaneOdyssey.Items.Armour.Sunken
{
	[AutoloadEquip(EquipType.Body)]
	public class SunkenChest : BaseArmour
	{
		public override ItemTiers ArmourTier => ItemTiers.Good;
		public override ushort AODefense => 194;
		public override short Size => AOAttkSpd;
		public override short AOAttkSpd => 22;
		public override ItemRarities Rarity => ItemRarities.Rare;

		public override int Value => 1350;
		public override SetBonusHelper? Set => GetSetBonusHelper("SunkenHelm", "SunkenBoots");

		public override void ArmorSetEffects(Player player)
		{
			player.GetModPlayer<SunkenPlayer>().sunkenSetBonus = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<SunkenScrap>(5).AddTile(TileID.MythrilAnvil).Register();
		}
	}

	public class SunkenPlayer : ModPlayer
	{
		public bool sunkenSetBonus = false;

		public override void ResetEffects()
		{
			sunkenSetBonus = false;
		}

		public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
		{
			if (sunkenSetBonus)
			{
				npc.AddBuff(BuffID.Wet, 60 * 10);
			}
		}
	}
}
