using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Items.Equipment.Scrolls
{
	public class ReflexScroll : EmptyScroll
	{
		public override void UpdateInventory(Player player)
		{
			if (player.TryGetImbue(out Imbuable imbue))
			{
				Item.color = imbue.GetColor() with { A = (byte)(255 * .75f) };
			}
			else Item.color = Color.Transparent;
		}

		public override bool CanUseItem(Player player)
		{
			return Item.ArcaneOdyssey().Imbue is not null;
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
				Item.color = imbue.GetColor() with { A = (byte)(255 * .75f) };
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
				player.ArcaneOdyssey().DashVelocity *= imbue.DashSpeed;
			}
		}

		public static float CalculateResistanceMulti(Player player) => player.Imbue()?.DashResist ?? 1f;

		public override bool OnHit(Player player, Entity target) => true;

		public override void DashEffect(Player player)
		{
			if (player.TryGetImbue(out Imbuable imbue))
			{
				imbue.LingeringEffects(player);
				player.statDefense *= CalculateResistanceMulti(player);
			}
		}

		public override float DashSpeed => 6;

		public override int DashMax => 30;

		public override bool Immune => false;
	}
}
