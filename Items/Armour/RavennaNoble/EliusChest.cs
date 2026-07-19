using ArcaneOdyssey.Items.Base;

namespace ArcaneOdyssey.Items.Armour.RavennaNoble
{
	[AutoloadEquip(EquipType.Body)]
	public class EliusChest : BaseArmour
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 30;
		}

		public override SetBonusHelper? Set => GetSetBonusHelper("EliusHelm", "EliusBoots");

		public override void ArmorSetEffects(Player player)
		{
			player.moveSpeed += .25f;
			player.jumpSpeedBoost += 2.5f;
		}

		public override ItemTiers ArmourTier => ItemTiers.Average;

		public override ItemRarities Rarity => ItemRarities.Uncommon;

		public override short AOPower => 10;

		public override short AOAgility => 12;

		public override int Value => 150;
	}
}
