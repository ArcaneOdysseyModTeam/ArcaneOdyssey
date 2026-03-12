using ArcaneOdyssey.Items.Base;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Accessories
{
	public class EvanderGauntlet : AOArmour
	{
		public override AORarities AORarity => AORarities.Uncommon;
		public override void Load()
		{
			if (!Main.dedServ)
			{
				EquipLoader.AddEquipTexture(Mod, Texture + "_HandsOff", EquipType.HandsOff, this);
				EquipLoader.AddEquipTexture(Mod, Texture + "_HandsOn", EquipType.HandsOn, this);
			}
		}
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			if (!Main.dedServ)
			{
				EquipLoader.GetEquipSlot(Mod, Name, EquipType.HandsOff);
				EquipLoader.GetEquipSlot(Mod, Name, EquipType.HandsOn);
			}
		}
		public override AOItemTiers ArmourTier => AOItemTiers.Good;
		public override int AODefense => 226;
		public override int AOPierce => 28;
		public override int AOValue => 200;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
		}

		public override void UpdateVanity(Player player)
		{
			base.UpdateVanity(player);
			player.GetModPlayer<GauntletPlayer>().equipped = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);
			if (!hideVisual)
			{
				player.GetModPlayer<GauntletPlayer>().equipped = true;
			}
		}
	}

	public class GauntletPlayer : ModPlayer
	{
		public bool equipped;
		public override void SetControls()
		{
			equipped = false;
		}

		public override void FrameEffects()
		{
			if (equipped)
			{
				if (Player.direction == -1)
				{
					Player.handon = EquipLoader.GetEquipSlot(Mod, nameof(EvanderGauntlet), EquipType.HandsOn);
				}
				else
				{
					Player.handoff = EquipLoader.GetEquipSlot(Mod, nameof(EvanderGauntlet), EquipType.HandsOff);
				}
			}
		}
	}
}
