using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace ArcaneOdyssey.Content.Items.Armour.Sunken
{
	[AutoloadEquip(EquipType.Legs)]
	public class SunkenBoots : AOArmour
	{
		public override AOItemTiers ArmourTier => AOItemTiers.Good;
		public override int AODefense => 145;
		public override int AOSize => AOAttkSpd;
		public override int AOAttkSpd => 16;
		public override AORarities AORarity => AORarities.Rare;

		public override int AOValue => 675;
		public override SetBonusHelper? Set => new(this, Color.Aqua, "SunkenHelm", "SunkenChest");

		public override void ArmorSetEffects(Player player)
		{
			player.GetModPlayer<SunkenPlayer>().sunkenSetBonus = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<SunkenScrap>(3).AddTile(TileID.MythrilAnvil).Register();
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
