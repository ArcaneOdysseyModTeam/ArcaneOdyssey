using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.VFX.Gores;
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
using static ArcaneOdyssey.AOUtils;
using static System.Net.Mime.MediaTypeNames;

namespace ArcaneOdyssey.Content.Items.Equipment.Scrolls
{
	public class ReflexScroll : EmptyScroll
	{
		public override void UpdateInventory(Player player)
		{
			if (player.TryGetImbue(out Imbuable imbue))
			{
				Item.color = imbue.ImbueColour;
			}
			else Item.color = Color.Transparent;
		}

		public override bool CanUseItem(Player player)
		{
			return Item.ArcaneOdyssey().imbue is not null;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (player.TryGetImbue(out Imbuable imbue))
			{
				Item.color = imbue.ImbueColour;
				player.ArcaneOdyssey().SetDash(new Reflex());
			}
			else Item.color = Color.Transparent;

		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<EmptyScroll>().AddIngredient(ItemID.EoCShield).Register();
			CreateRecipe().AddIngredient<EmptyScroll>().AddIngredient(ItemID.CobaltShield).Register();
		}
	}

	public class Reflex : DashSystem
	{
		public override int Cooldown => 30;

		public override bool AnyDirection => false;

		public override void OnStart(Player player)
		{
			if (player.TryGetImbue(out Imbuable imbue))
			{
				SoundEngine.PlaySound(imbue.ImbueSound, player.MountedCenter);
				player.ArcaneOdyssey().DashVelocity *= imbue.AOScrollSpeed;
			}
		}

		public override bool OnHit(Player player, Entity target)
		{
			return true;
		}

		public override void DashEffect(Player player)
		{
			if (player.TryGetImbue(out Imbuable imbue))
			{
				imbue.LingeringEffects(player);
				player.gravity = 0f;
				player.position.Y -= 0.6f * player.velocity.Y;
			} 
		}

		public override float DashSpeed => 10;

		public override int DashMax => 30;

		public override bool Immune => false;
	}
}
