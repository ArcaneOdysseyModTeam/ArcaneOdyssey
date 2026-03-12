using ArcaneOdyssey.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Armour.RavennaNoble
{
	[AutoloadEquip(EquipType.Body)]
	public class EliusChest : AOArmour
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			if (Main.netMode != NetmodeID.Server)
			{
				EquipLoader.GetEquipSlot(Mod, Name, EquipType.Back);
			}
		}

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

		public override AOItemTiers ArmourTier => AOItemTiers.Average;

		public override AORarities AORarity => AORarities.Rare;

		public override int AOPower => 10;

		public override int AOAgility => 12;

		public override int AOValue => 150;

		public override void Load()
		{
			if (!Main.dedServ)
			{
				EquipLoader.AddEquipTexture(Mod, Texture + "_" + EquipType.Back, EquipType.Back, this);
			}
		}
	}
}
