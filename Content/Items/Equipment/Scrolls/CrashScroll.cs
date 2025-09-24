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

namespace ArcaneOdyssey.Content.Items.Equipment.Scrolls
{
	public class CrashScroll : TechniqueScroll
	{
		public override void SetDefaultsScroll()
		{
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			AOPlayer playah = player.ArcaneOdyssey();
			Item.ArcaneOdyssey().imbue = playah.imbue;
			if (playah.imbue is FightingStyle)
			{
				Item.color = playah.imbue.ImbueColour;
				player.DashPlayer().dash ??= new CrashDash();
			}
			else Item.color = Color.Transparent;

		}
		public override void ScrollRecipe()
		{
			CreateRecipe().AddIngredient<EmptyScroll>().AddRecipeGroup(RecipeGroupID.Balloons).Register();
		}
	}

	public class CrashDash : DashSystem
	{
		public override int Cooldown => 60 * 10;

		public override bool AnyDirection => true;

		public override int Damage => 50;

		public override bool OnHit(Player player, Entity target)
		{
			return true;
		}
		public override void OnEnd(Player player)
		{
			player.velocity = Vector2.Zero;
		}

		public override float DashSpeed => 15;

		public override int DashMax => 15;

		public override float Knockback => 2f;

		public override string Name => "Crash";

		public override bool Immune => true;
	}
}
