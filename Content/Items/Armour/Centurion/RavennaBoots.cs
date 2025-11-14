using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Armour.Centurion
{
	[AutoloadEquip(EquipType.Legs)]
	public class RavennaBoots : AOArmour
    {
        public override AOItemTiers ArmourTier => AOItemTiers.Poor;

        public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			if (Main.netMode != NetmodeID.Server)
			{
				EquipLoader.GetEquipSlot(Mod, Name, EquipType.Shield);
			}
		}

		public override void Load()
		{
			if (Main.netMode != NetmodeID.Server)
			{
				EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Shield}", EquipType.Shield, this);
			}
		}

		public override int AODefense => 56;
		public override int AOSize => AODefense / 20;
		public override int AOAttkSpd => AODefense / 20;
		public override AORarities AORarity => AORarities.Common;
		public override int AOValue => 30;

		public override SetBonusHelper? Set => new(Mod, "Ravenna Bulwark", "Allows you to brace, slowing your movement in exchange for a defence bonus", ["RavennaHelm", "RavennaChest"], Color.Orange);

		public override void ArmorSetEffects(Player player)
		{
			player.GetModPlayer<CenturionPlayer>().bronzeSetBonus = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BronzeBar>(15).AddTile(TileID.Anvils).Register();
		}
	}

	public class CenturionPlayer : ModPlayer
	{
		public bool bronzeSetBonus = false;
		public bool bracing = false;

		public override void ResetEffects()
		{
			if (!bronzeSetBonus)
				bracing = false;
			bronzeSetBonus = false;
		}

		public override void ArmorSetBonusActivated()
		{
			if (bronzeSetBonus)
			{
				bracing = !bracing;
			}
		}

		public override void PostUpdateRunSpeeds()
		{
			if (bracing)
			{
				Player.moveSpeed -= .5f;
				Player.statDefense *= 1.2f;
			}
		}

		public override void FrameEffects()
		{
			if (bracing)
			{
				Player.shield = EquipLoader.GetEquipSlot(Mod, typeof(RavennaBoots).Name, EquipType.Shield);
			}
		}
	}
}
