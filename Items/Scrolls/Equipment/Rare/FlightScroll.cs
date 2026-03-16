using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Scrolls.Equipment.Rare
{
	[AutoloadEquip(EquipType.Wings)]
	public class FlightScroll : RareScroll
	{
		public override bool CanHaveMagic => true;
		public override bool CanHaveRelic => true;


		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			if (HasCorrectImbue)
			{
				constantAscend *= Imbue.AOScrollSpeed;
				ascentWhenRising *= Imbue.AOScrollSpeed;
				maxCanAscendMultiplier *= Imbue.AOScrollSpeed;
				maxAscentMultiplier *= Imbue.AOScrollSpeed;
			}
			else
			{
				ascentWhenFalling *= 0f;
				ascentWhenRising *= 0f;
				constantAscend *= 0f;
				player.velocity.Y = player.maxFallSpeed;
				player.wingTime *= 0f;
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
				speed *= 0;
				acceleration *= 0;
			}
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(150);
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (HasCorrectImbue)
			{
				player.noFallDmg = true;
			}
			else if (!player.mount.Active)
			{
				player.wingTime = 0;
				player.equippedWings = null;
			}
		}

		public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
		{
			return player.Imbue() is not FightingStyle or null;
		}

		public override bool WingUpdate(Player player, bool inUse)
		{
			if (inUse)
			{
				player.Imbue()?.LingeringEffects(player.Hitbox.Scaled(3f));
				player.Imbue()?.Imbue?.LingeringEffects(player.Hitbox.Scaled(3f));
			}

			return false;
		}
	}
}
