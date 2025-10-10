using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace ArcaneOdyssey.Content.Items.Armour.Sunken
{
	[AutoloadEquip(EquipType.Legs)]
	public class SunkenBoots : AOArmour
	{
		public override int AODefense => 204;
		public override int AOSize => 23;
		public override int AOAttkSpd => 23;
		public override AORarities AORarity => AORarities.Rare;

		public override int AOAgility => 30;
		public override int AOValue => 1350;
		public override SetBonusHelper? Set => new(Mod, "Aquatic Rebuttal", "Enemies that strike you are soaked", ["SunkenHelm", "SunkenChest"], Color.Aqua);

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 38;
		}

		public override void ArmorSetEffects(Player player)
		{
			player.GetModPlayer<SunkenPlayer>().sunkenSetBonus = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<ArcaniumScrap>(3).AddTile(TileID.MythrilAnvil).Register();
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
