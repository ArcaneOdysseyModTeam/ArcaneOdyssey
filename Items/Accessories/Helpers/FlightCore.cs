using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Scrolls.Equipment.Rare;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Accessories.Helpers
{
	[AutoloadEquip(EquipType.Wings)]
	public class FlightCore : BaseItem, IImbuable
	{
		public Imbuable Imbue
		{
			get
			{
				return Item?.ArcaneOdyssey()?.Imbue;
			}
			set
			{
				if (Item?.ArcaneOdyssey() is not null)
				{
					Item.ArcaneOdyssey().Imbue = value;
				}
			}
		}

		public Imbuable SecondImbue
		{
			get
			{
				return Item?.ArcaneOdyssey()?.SecondImbue;
			}
			set
			{
				if (Item?.ArcaneOdyssey() is not null)
				{
					Item.ArcaneOdyssey().SecondImbue = value;
				}
			}
		}

		public override ItemRarities Rarity => ItemRarities.Unknown;

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			if (Imbue is not null)
			{
				constantAscend *= Imbue.ScrollSpeed;
				ascentWhenRising *= Imbue.ScrollSpeed;
				maxCanAscendMultiplier *= Imbue.ScrollSpeed;
				maxAscentMultiplier *= Imbue.ScrollSpeed;
				if (SecondImbue is not null)
				{
					constantAscend *= SecondImbue.ScrollSpeed;
					ascentWhenRising *= SecondImbue.ScrollSpeed;
					maxCanAscendMultiplier *= SecondImbue.ScrollSpeed;
					maxAscentMultiplier *= SecondImbue.ScrollSpeed;
				}
			}
		}

		public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
		{
			if (Imbue is not null)
			{
				speed *= Imbue.ScrollSpeed;
				acceleration *= Imbue.ScrollSpeed;
				if (SecondImbue is not null)
				{
					speed *= SecondImbue.ScrollSpeed;
					acceleration *= SecondImbue.ScrollSpeed;
				}
			}
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(150);
			ArcaneOdysseyMod.Sets.showItemTypeTooltip[Type] = false;
			ItemID.Sets.IgnoresEncumberingStone[Type] = true;
			ItemID.Sets.CanGetPrefixes[Type] = false;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
			Item.uniqueStack = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (Imbue is not null)
			{
				player.noFallDmg = true;
				Item.color = Imbue.Colour * .75f;
			}
			else
			{
				Item.TurnToAir();
			}
		}

		public override void UpdateInventory(Player player)
		{
			if (Imbue is not null)
				Item.color = Imbue.Colour * .75f;
		}

		public override bool WingUpdate(Player player, bool inUse)
		{
			if (inUse)
			{
				Imbue?.LingeringEffects(player.Hitbox.Scaled(3f));
				SecondImbue?.LingeringEffects(player.Hitbox.Scaled(3f));
			}

			return false;
		}

		private int airTime = 1;
		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			if (airTime-- <= 0)
				Item.TurnToAir();
		}
	}
}
