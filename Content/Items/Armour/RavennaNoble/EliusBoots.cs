using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Armour.RavennaNoble
{
	[AutoloadEquip(EquipType.Legs)]
	public class EliusBoots : AOArmour
	{
		public override AOItemTiers ArmourTier => AOItemTiers.Average;
		public override int AODefense => 70;
		public override AORarities AORarity => AORarities.Rare;
		public override int AOValue => 60;
		public override int AOAgility => 11;
		public override int AOPower => 9;

		public override SetBonusHelper? Set => new(this, Color.MediumPurple, "EliusHelm", "EliusChest");

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = 26;
			Item.height = 16;
		}
	}
}
