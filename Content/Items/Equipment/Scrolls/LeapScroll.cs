using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

namespace ArcaneOdyssey.Content.Items.Equipment.Scrolls
{
	public class LeapScroll : MagicScroll
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
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
		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<EmptyScroll>().AddRecipeGroup(RecipeGroupID.Balloons).Register();
			CreateRecipe().AddIngredient<EmptyScroll>().AddIngredient(ItemID.PinkGel).Register();
		}
	}

	public class LeapAirStep : ExtraJump
	{
		public override Position GetDefaultPosition()
		{
			return BeforeBottleJumps;
		}

		public override float GetDurationMultiplier(Player player) => player.Imbue().AOScrollSize * 2;

		public override void UpdateHorizontalSpeeds(Player player)
		{
			player.runAcceleration *= (player.Imbue().AOScrollSpeed + 1) * 2;
			player.maxRunSpeed *= player.Imbue().AOScrollSpeed + 1;
			player.jumpSpeedBoost *= player.Imbue().AOScrollSpeed;
			base.UpdateHorizontalSpeeds(player);
		}

		public override bool CanStart(Player player)
		{
			return player.Imbue() is not null;
		}

		public override void OnStarted(Player player, ref bool playSound)
		{
			player.ChangeDir((player.velocity.X > 0).ToDirectionInt());
			if (player.Imbue() is AOMagic)
			{
				var item = new Item(ModContent.ItemType<LeapScroll>());
				item.ArcaneOdyssey().imbue = player.Imbue();
				var proj = AOMagic.CreateMagicCircle(item, player, player.Imbue());
				for (int i = 0; i < 5; i++)
					player.Imbue().ExplosionEffects(proj);
			}

			if (player.Imbue().ImbueSound.HasValue)
			{
				SoundEngine.PlaySound(player.Imbue().ImbueSound, player.Bottom);
				playSound = false;
			}
			// vfx here
		}
	}
}
