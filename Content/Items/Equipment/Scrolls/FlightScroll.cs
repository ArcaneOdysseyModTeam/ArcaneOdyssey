using ArcaneOdyssey.Content.Items.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using ArcaneOdyssey.Content.Items.Materials;

namespace ArcaneOdyssey.Content.Items.Equipment.Scrolls
{
	[AutoloadEquip(EquipType.Wings)]
	public class FlightScroll : MagicScroll
	{
		public override int AOValue => 1000;

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			if (player.TryGetImbue(out var imbue) && imbue is AOMagic)
			{
				constantAscend *= imbue.AOScrollSpeed;
				ascentWhenRising *= imbue.AOScrollSpeed;
				maxCanAscendMultiplier *= imbue.AOScrollSpeed;
				maxAscentMultiplier *= imbue.AOScrollSpeed;
			}
		}

		public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
		{
			if (player.TryGetImbue(out var imbue) && imbue is AOMagic)
			{
				speed *= imbue.AOScrollSpeed;
				acceleration *= imbue.AOScrollSpeed;
			}
		}

		public override void SetStaticDefaults()
		{
			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(180, hasHoldDownHoverFeatures: true);
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
		}

		public override void UpdateEquip(Player player)
		{
			if (player.Imbue() is AOMagic)
			{
				player.noFallDmg = true;
			}
			else
			{
				player.slowFall = false;
				player.wingTime = 0;
			}
		}

		public override bool WingUpdate(Player player, bool inUse)
		{
			if (player.TryGetImbue(out var imbue) && inUse && imbue is AOMagic)
			{
				imbue.LingeringEffects(player);
			}

			return base.WingUpdate(player, inUse);
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient(ItemID.SoulofFlight, 20).AddIngredient<EmptyScroll>().Register();
		}
	}
}
