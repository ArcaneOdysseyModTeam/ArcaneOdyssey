using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Armour.RavennaNoble
{
	[AutoloadEquip(EquipType.Body)]
	public class EliusChest : Base.Armour
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 30;
		}

		public override SetBonusHelper? Set => new(this, Color.MediumPurple, "EliusHelm", "EliusBoots");

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
