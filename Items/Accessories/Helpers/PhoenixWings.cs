using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.Items.Base;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Items.Accessories.Helpers
{
	[AutoloadEquip(EquipType.Wings)]
	public class PhoenixWings : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Legendary;

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

		public override bool WingUpdate(Player player, bool inUse)
		{
			Vector2 spawnPos = player.MountedCenter + new Vector2(-25 * player.direction, 0);
			Lighting.AddLight(spawnPos, PhoenixMagic.Instance.Colour.ToVector3() * 1.5f);
			return base.WingUpdate(player, inUse);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);
			if (Item.active)
			{
				player.wingsLogic = 0;
			}
			else
			{
				player.noFallDmg = true;
			}
		}

		public override bool ModifyEquipTextureDraw(ref PlayerDrawSet drawInfo, ref DrawData drawData, EquipTexture equipTexture, string methodName)
		{
			drawData.color = Color.White * (1f - drawInfo.shadow);
			return true;
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(180, 8f, 2f, true, 12f, 12f);
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
			Item.vanity = true;
		}
	}
}
