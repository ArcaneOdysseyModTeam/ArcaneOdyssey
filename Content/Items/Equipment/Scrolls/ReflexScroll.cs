using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

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
			if (Imbue is not null)
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
		
		public override int Cooldown => 30;

		public override bool AnyDirection => false;

		public override void OnStart(Player player)
		{
			if (player.TryGetImbue(out Imbuable imbue))
			{
				SoundEngine.PlaySound(imbue.ImbueSound, player.MountedCenter);
			}
		}

		public override bool OnHit(Player player, Entity target) => true;

		public override void DashEffect(Player player)
		{
			player.statDefense *= Imbue?.DashResist ?? 1f;
		}

		public override float DashSpeed => 6;

		public override int DashMax => 30;

		public override bool Immune => false;
	}
}
