using ArcaneOdyssey.Items.Base;

namespace ArcaneOdyssey.Items.Accessories.Vanity
{
	[AutoloadEquip(EquipType.Back)]
	public class KindraBlade : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Special;

		public override void Load()
		{
			if (Main.dedServ)
				return;
			EquipLoader.AddEquipTexture(Mod, Texture + "_Body", EquipType.Body, this);
			EquipLoader.AddEquipTexture(Mod, Texture + "_Legs", EquipType.Legs, this);
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 40;
			Item.accessory = true;
			Item.vanity = true;
		}

		public override void EquipFrameEffects(Player player, EquipType type)
		{
			if (Main.dedServ)
				return;
			player.legs = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);
			player.body = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
		}
	}
}
