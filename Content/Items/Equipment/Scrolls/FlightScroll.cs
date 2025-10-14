using ArcaneOdyssey.Content.Items.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using static ArcaneOdyssey.AOUtils;
using ArcaneOdyssey.Content.Items.Materials;

namespace ArcaneOdyssey.Content.Items.Equipment.Scrolls
{
	[AutoloadEquip(EquipType.Wings)]
	public class FlightScroll : MagicScroll
	{
		public override int AOValue => 1000;

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			if (!player.GetModPlayer<HoverPlayer>().hasHoverEquipped) // slow fall disabled without hover spell lol
			{
				ascentWhenFalling = 0;
			}
			if (player.TryGetImbue(out var imbue))
			{
                constantAscend *= imbue.AOScrollSpeed;
				ascentWhenRising *= imbue.AOScrollDamage;
				maxAscentMultiplier *= imbue.AOScrollSize;
			}
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            AOPlayer playah = player.ArcaneOdyssey();
            Item.ArcaneOdyssey().imbue = playah.imbue;
            if (playah.imbue is AOMagic)
            {
                Item.color = playah.imbue.ImbueColour;
                player.GetJumpState<LeapAirStep>().Enable();
            }
            else Item.color = Color.Transparent;

        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
		{
			if (player.TryGetImbue(out var imbue))
			{
				speed *= imbue.AOScrollSpeed;
				acceleration *= imbue.AOScrollDamage;
			}
		}

		public override void SetStaticDefaults()
		{
			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(180, 6.5f, 1.5f);
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
		}

		public override void UpdateEquip(Player player)
		{
			if (player.TryGetImbue(out _))
				player.noFallDmg = true;
			else
				player.wings = 0;
		}

		public override bool CanEquipAccessory(Player player, int slot, bool modded)
		{
			return player.Imbue() is not null;
		}

		public override bool WingUpdate(Player player, bool inUse)
		{
			if (player.TryGetImbue(out var imbue) && inUse)
			{
				imbue.LingeringEffects(player);
			}

			return base.WingUpdate(player, inUse);
		}

		public override void AddRecipes()
		{
			var group = new RecipeGroup(() => Mod.CustomLocalization("AnyWings").Value, [
				ItemID.DemonWings,
				ItemID.AngelWings,
				ItemID.RedsWings,
				ItemID.ButterflyWings,
				ItemID.FairyWings,
				ItemID.HarpyWings,
				ItemID.BoneWings,
				ItemID.FlameWings,
				ItemID.FrozenWings,
				ItemID.GhostWings,
				ItemID.SteampunkWings,
				ItemID.LeafWings,
				ItemID.BatWings,
				ItemID.BeeWings,
				ItemID.DTownsWings,
				ItemID.WillsWings,
				ItemID.CrownosWings,
				ItemID.CenxsWings,
				ItemID.TatteredFairyWings,
				ItemID.SpookyWings,
				ItemID.Hoverboard,
				ItemID.FestiveWings,
				ItemID.BeetleWings,
				ItemID.FinWings,
				ItemID.FishronWings,
				ItemID.MothronWings,
				ItemID.WingsSolar,
				ItemID.WingsVortex,
				ItemID.WingsNebula,
				ItemID.WingsStardust,
				ItemID.Yoraiz0rWings,
				ItemID.JimsWings,
				ItemID.SkiphsWings,
				ItemID.LokisWings,
				ItemID.BetsyWings,
				ItemID.ArkhalisWings,
				ItemID.LeinforsWings,
				ItemID.BejeweledValkyrieWing,
				ItemID.GhostarsWings,
				ItemID.GroxTheGreatWings,
				ItemID.FoodBarbarianWings,
				ItemID.SafemanWings,
				ItemID.CreativeWings,
				ItemID.RainbowWings,
				ItemID.LongRainbowTrailWings,
			]);
			var any = RecipeGroup.RegisterGroup("AnyWings", group);
			CreateRecipe().AddRecipeGroup(any).AddIngredient<EmptyScroll>().Register();
		}
	}
}
