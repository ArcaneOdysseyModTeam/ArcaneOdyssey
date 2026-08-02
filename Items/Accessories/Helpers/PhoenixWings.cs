using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.Items.Base;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Items.Accessories.Helpers
{
	[AutoloadEquip(EquipType.Wings)]
	public class PhoenixWings : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Unknown;

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = PhoenixMagic.AscentWhenFalling;
			ascentWhenRising = PhoenixMagic.AscentWhenRising;
			maxCanAscendMultiplier = PhoenixMagic.MaxCanAscendMultiplier;
			maxAscentMultiplier = PhoenixMagic.MaxAscentMultiplier;
			constantAscend = PhoenixMagic.ConstantAscend;

			if (player.TryingToHoverDown && player.controlJump && player.wingTime > 0f && !player.merman)
			{
				player.wingTime += 0.5f;
				player.velocity.Y *= 0.8f;
				if (player.velocity.Y > -2f && player.velocity.Y < 1f)
					player.velocity.Y = 0.00001f;
				ascentWhenFalling *= 0f;
				ascentWhenRising *= 0f;
				constantAscend *= 0f;
			}
		}

		public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
		{
			var result = base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
			if (result)
				player.ArcaneOdyssey().hasWings = 2;
			return result;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);
			player.ArcaneOdyssey().hasWings = 2;
			if (player.Imbue() is PhoenixMagic && player.HasTypeInInventory<PhoenixMagic>(e => e.Mobility is PhoenixFlight))
			{
				player.noFallDmg = true;
				if (!hideVisual)
				{
					Vector2 spawnPos = player.MountedCenter + new Vector2(-25 * player.direction, 0);
					Lighting.AddLight(spawnPos, PhoenixMagic.Instance.Colour.ToVector3() * 1.5f);
				}
			}
			else
			{
				Item.TurnToAir(true);
			}
		}

		public override bool ModifyEquipTextureDraw(ref PlayerDrawSet drawInfo, ref DrawData drawData, EquipTexture equipTexture, string methodName)
		{
			drawData.color = Color.White * (1f-drawInfo.shadow);
			return true;
		}

		public override void UpdateInventory(Player player)
		{
			base.UpdateInventory(player);
			if (player.Imbue() is not PhoenixMagic || !player.HasTypeInInventory<PhoenixMagic>(e => e.Mobility is PhoenixFlight))
			{
				Item.TurnToAir();
			}
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(180, 8f, 2f, true, 12f, 12f);
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

		private int airTime = 1;
		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			if (airTime-- <= 0)
				Item.TurnToAir();
		}
	}
}
