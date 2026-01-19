using ArcaneOdyssey.Content.Items.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using ArcaneOdyssey.Content.Items.Materials;

namespace ArcaneOdyssey.Content.Items.Equipment.Scrolls
{
	[AutoloadEquip(EquipType.Wings)]
	public class FlightScroll : Scroll
	{
		public override bool CanHaveMagic => true;
		public override bool CanHaveRelic => true;

		public override int AOValue => 1000;

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			if (HasCorrectImbue)
			{
				constantAscend *= Imbue.AOScrollSpeed;
				ascentWhenRising *= Imbue.AOScrollSpeed;
				maxCanAscendMultiplier *= Imbue.AOScrollSpeed;
				maxAscentMultiplier *= Imbue.AOScrollSpeed;
			}
		}

		public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
		{
			if (HasCorrectImbue)
			{
				speed *= Imbue.AOScrollSpeed;
				acceleration *= Imbue.AOScrollSpeed;
			}
			else
			{
				speed = 0;
			}
		}

		public override void SetStaticDefaults()
		{
			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(180);
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
		}

		public override void UpdateEquip(Player player)
		{
			if (HasCorrectImbue)
			{
				player.noFallDmg = true;
			}
			else
			{
				player.wingTime = 0;
				player.equippedWings = null;
			}
		}

		public override bool WingUpdate(Player player, bool inUse)
		{
			if (inUse)
			{
				player.Imbue()?.LingeringEffects(player);
				player.Imbue()?.Imbue?.LingeringEffects(player);
			}

			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient(ItemID.SoulofFlight, 20).AddIngredient<EmptyScroll>().Register();
		}
	}
}
