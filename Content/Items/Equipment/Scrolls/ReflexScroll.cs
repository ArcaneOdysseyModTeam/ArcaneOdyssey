using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Normal;
using ArcaneOdyssey.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using ArcaneOdyssey.PlayerClasses;

namespace ArcaneOdyssey.Content.Items.Equipment.Scrolls
{
	public class ReflexScroll : Scroll
	{
		public override bool CanHaveRelic => true;
		public override bool CanHaveFS => true;
		public override bool CanHaveMagic => true;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);
			if (HasCorrectImbue)
				player.ArcaneOdyssey()?.SetDash(new Reflex(Item));
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<EmptyScroll>().AddIngredient(ItemID.EoCShield).Register();
			CreateRecipe().AddIngredient<EmptyScroll>().AddIngredient(ItemID.CobaltShield).Register();
		}
	}

	public class Reflex(Entity source) : DashSystem(source)
	{
		private float invisbase;

		public override int Cooldown => 30;

		public override bool AnyDirection => false;

		public override void OnStart(Player player)
		{
			if (Imbue is not null)
			{
				SoundEngine.PlaySound(Imbue.ImbueSound, player.MountedCenter);
				if (Imbue is VanishingStyle)
				{
					invisbase = player.opacityForAnimation;
				}
			}
		}

		public override bool OnHit(Player player, Entity target) => true;

		public override void DashEffect(Player player)
		{
			if (Imbue?.DashResist.HasValue == true)
				player.statDefense *= Imbue.DashResist.Value;

			if (Imbue is VanishingStyle)
				player.opacityForAnimation = MathHelper.Lerp(invisbase, 0f, player.ArcaneOdyssey().DashLerp);
		}

		public override void OnEnd(Player player)
		{
			player.opacityForAnimation = 1f;
		}

		public override float DashSpeed => 15;

		public override int DashMax => 30;

		public override bool Immune => false;
	}
}
