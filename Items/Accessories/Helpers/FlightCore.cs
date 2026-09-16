using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Items.Base;
using Terraria.Audio;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Items.Accessories.Helpers
{
	[AutoloadEquip(EquipType.Wings)]
	public class FlightCore : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Mystic;

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			if (player.Imbue() is not null)
			{
				constantAscend *= player.Imbue().ScrollSpeed;
				ascentWhenRising *= player.Imbue().ScrollSpeed;
				maxCanAscendMultiplier *= player.Imbue().ScrollSpeed;
				maxAscentMultiplier *= player.Imbue().ScrollSpeed;
				if (player.SecondImbue() is not null)
				{
					constantAscend *= player.SecondImbue().ScrollSpeed;
					ascentWhenRising *= player.SecondImbue().ScrollSpeed;
					maxCanAscendMultiplier *= player.SecondImbue().ScrollSpeed;
					maxAscentMultiplier *= player.SecondImbue().ScrollSpeed;
				}
			}
		}

		public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
		{
			if (player.Imbue() is not null)
			{
				speed *= player.Imbue().ScrollSpeed;
				acceleration *= player.Imbue().ScrollSpeed;
				if (player.SecondImbue() is not null)
				{
					speed *= player.SecondImbue().ScrollSpeed;
					acceleration *= player.SecondImbue().ScrollSpeed;
				}
			}
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(150);
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

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
			Item.vanity = true;
		}

		public override bool WingUpdate(Player player, bool inUse)
		{
			if (inUse)
			{
				Imbuable.RequestMobilityCircle(player.Imbue()?.Item, player, Projectiles.MobilityCircleMode.Flight, false);

				if (player.Imbue() is SpiritEnergy imbue)
				{
					player.Imbue()?.LingeringEffects(player.Hitbox.Scaled(3f));
					player.Imbue()?.Imbue?.LingeringEffects(player.Hitbox.Scaled(3f));
				}

				if (!player.flapSound && player.Imbue()?.ImbueSound.HasValue == true)
				{
					SoundEngine.PlaySound(player.Imbue().ImbueSound.Value with { MaxInstances = 1, Volume = player.Imbue().ImbueSound.Value.Volume / 2f, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, player.Center);
				}

				player.flapSound = true;
			}

			return false;
		}

		public override bool ModifyEquipTextureDraw(ref PlayerDrawSet drawInfo, ref DrawData drawData, EquipTexture equipTexture, string methodName)
		{
			return false;
		}
	}
}
