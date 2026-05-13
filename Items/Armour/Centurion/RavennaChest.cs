using ArcaneOdyssey.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Armour.Centurion
{
	[AutoloadEquip(EquipType.Body)]
	public class RavennaChest : Base.Armour
	{
		public override ItemTiers ArmourTier => ItemTiers.Average;
		public override ushort AODefense => 197;
		public override short Size => (short)(AODefense / 17);
		public override short AOAttkSpd => (short)(AODefense / 17);
		public override ItemRarities Rarity => ItemRarities.Uncommon;

		public override int Value => 110;

		public override void SetDefaults()
		{
			base.SetDefaults();
		}

		public override SetBonusHelper? Set => new(this, Color.Orange, "RavennaHelm", "RavennaBoots");

		public override void ArmorSetEffects(Player player)
		{
			player.GetModPlayer<CenturionPlayer>().bronzeSetBonus = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BronzeBar>(20).AddTile(TileID.Anvils).Register();
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
				Player.accRunSpeed /= 2f;
				Player.maxRunSpeed /= 2f;
				Player.statDefense *= 1.25f;
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
